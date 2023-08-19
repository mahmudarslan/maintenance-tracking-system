using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Arslan.Vms.IdentityService.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class IdentityServiceDbContextFactory : IDesignTimeDbContextFactory<IdentityServiceDbContext>
{
    public IdentityServiceDbContext CreateDbContext(string[] args)
    {
        IdentityServiceEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<IdentityServiceDbContext>()
            .UseSqlServer(
            configuration.GetConnectionString(IdentityServiceDbProperties.ConnectionStringName),
            b =>
            {
                b.MigrationsHistoryTable("__IdentityService_Migrations");
            });

        return new IdentityServiceDbContext(builder.Options);
    } 

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Arslan.Vms.IdentityService.HttpApi.Host/"))
                   .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}