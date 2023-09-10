using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Arslan.Vms.InventoryService.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class InventoryServiceDbContextFactory : IDesignTimeDbContextFactory<InventoryServiceDbContext>
{
    public InventoryServiceDbContext CreateDbContext(string[] args)
    {
        InventoryServiceEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<InventoryServiceDbContext>()
            .UseSqlServer(
            configuration.GetConnectionString(InventoryServiceDbProperties.ConnectionStringName),
            b =>
            {
                b.MigrationsHistoryTable("__InventoryService_Migrations");
            });

        return new InventoryServiceDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Arslan.Vms.InventoryService.HttpApi.Host/"))
                   .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}