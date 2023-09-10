using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Arslan.Vms.OrderService.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class OrderServiceDbContextFactory : IDesignTimeDbContextFactory<OrderServiceDbContext>
{
    public OrderServiceDbContext CreateDbContext(string[] args)
    {
        OrderServiceEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<OrderServiceDbContext>()
            .UseSqlServer(
            configuration.GetConnectionString(OrderServiceDbProperties.ConnectionStringName),
            b =>
            {
                b.MigrationsHistoryTable("__OrderService_Migrations");
            });

        return new OrderServiceDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Arslan.Vms.OrderService.HttpApi.Host/"))
                   .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}