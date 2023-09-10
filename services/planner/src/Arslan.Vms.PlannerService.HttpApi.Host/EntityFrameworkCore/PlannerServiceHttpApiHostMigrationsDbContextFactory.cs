using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Arslan.Vms.PlannerService.EntityFrameworkCore;

public class PlannerServiceHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<PlannerServiceHttpApiHostMigrationsDbContext>
{
    public PlannerServiceHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
    {
        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<PlannerServiceHttpApiHostMigrationsDbContext>()
            .UseSqlServer(configuration.GetConnectionString("PlannerService"));

        return new PlannerServiceHttpApiHostMigrationsDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
