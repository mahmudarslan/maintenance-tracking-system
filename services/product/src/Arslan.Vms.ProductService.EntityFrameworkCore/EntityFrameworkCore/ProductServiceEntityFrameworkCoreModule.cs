using Arslan.Vms.ProductService.Accounts;
using Arslan.Vms.ProductService.Products;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Arslan.Vms.ProductService.EntityFrameworkCore;

[DependsOn(
    typeof(ProductServiceDomainModule),
    typeof(AbpEntityFrameworkCoreModule)
)]
public class ProductServiceEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<ProductServiceDbContext>(options =>
        {
            /* Add custom repositories here. Example:
             * options.AddRepository<Question, EfCoreQuestionRepository>();
             */

            options.AddDefaultRepositories<IProductServiceDbContext>();

            options.AddRepository<Product, ProductRepository>();
            options.AddRepository<ProductPrice, ProductPriceRepository>();
            options.AddRepository<Checkpoint, CheckpointRepository>();
            options.AddRepository<Balances, BalanceRepository>();
        });
    }
}
