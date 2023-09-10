using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Arslan.Vms.VehicleService.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class VehicleServiceDbContextFactory : IDesignTimeDbContextFactory<VehicleServiceDbContext>
{
    public VehicleServiceDbContext CreateDbContext(string[] args)
    {
        VehicleServiceEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<VehicleServiceDbContext>()
            .UseSqlServer(
            configuration.GetConnectionString(VehicleServiceDbProperties.ConnectionStringName),
            b =>
            {
                b.MigrationsHistoryTable("__VehicleService_Migrations");
            });

        return new VehicleServiceDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Arslan.Vms.VehicleService.HttpApi.Host/"))
                   .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}