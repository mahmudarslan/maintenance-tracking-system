using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Arslan.Vms.ProductService.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class ProductServiceDbContextFactory : IDesignTimeDbContextFactory<ProductServiceDbContext>
{
    public ProductServiceDbContext CreateDbContext(string[] args)
    {
        ProductServiceEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<ProductServiceDbContext>()
            .UseSqlServer(
            configuration.GetConnectionString(ProductServiceDbProperties.ConnectionStringName),
            b =>
            {
                b.MigrationsHistoryTable("__ProductService_Migrations");
            });

        return new ProductServiceDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Arslan.Vms.ProductService.HttpApi.Host/"))
                   .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}