using Arslan.Vms.AdministrationService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.DbIntegration;

public class DbIntegrationHostedService : IHostedService
{
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly IConfiguration _configuration;

    public DbIntegrationHostedService(IHostApplicationLifetime hostApplicationLifetime,
        IConfiguration configuration)
    {
        _hostApplicationLifetime = hostApplicationLifetime;
        _configuration = configuration;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using (var application = AbpApplicationFactory.Create<VmsDbIntegrationModule>(options =>
    {
        options.Services.ReplaceConfiguration(_configuration);
        options.UseAutofac();
        options.Services.AddLogging(c => c.AddSerilog());
    }))
            {
                application.Initialize();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                var _currentTenant = application.ServiceProvider.GetRequiredService<ICurrentTenant>();

                using (_currentTenant.Change(MigrationConst.DemoTenantId))
                {
                    Console.WriteLine($"Administration data transfer started");
                    await UsingUowAsync(application, async () => { await (new AdministrationServiceIntegration(application)).SeedDataAsync(); }, "Administration");
                    Console.WriteLine($"--------------------------------------------------------");
    
                    Console.WriteLine($"--------------------------------------------------------");
                    Log.Information("All data transfer is finished");

                    stopwatch.Stop();

                    Console.WriteLine($"All data transfer is finished");

                    Console.WriteLine($"Time elapsed (For): {stopwatch.Elapsed}");
                    Console.WriteLine($"Total Hours: {stopwatch.Elapsed.TotalHours}");
                    Console.WriteLine($"Total Minutes:  {stopwatch.Elapsed.TotalMinutes}");
                    Console.WriteLine($"");
                    Console.WriteLine($"--------------------------------------------------------");

                    Console.ReadLine();//wait...

                    while (true)
                    {
                        Thread.Sleep(TimeSpan.FromMinutes(60));
                    }
                }

                application.Shutdown();

                _hostApplicationLifetime.StopApplication();
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    protected virtual async Task UsingUowAsync(IAbpApplicationWithInternalServiceProvider application, Func<Task> action, string title = "")
    {
        try
        {
            using (var uow = application.ServiceProvider.GetRequiredService<IUnitOfWorkManager>().Begin(true))
            {
                Log.Information($"[start] DbIntegrationHostedService->{title} Data seeding...");

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                await action();
                await uow.CompleteAsync();

                stopwatch.Stop();

                Log.Information($"[end] DbIntegrationHostedService->{title} total time(ms): {stopwatch.ElapsedMilliseconds}");
            }
        }
        catch (Exception ex)
        {
            Log.Error($"DbIntegrationHostedService->{title} : {ex.Message}");
        }
    }
}