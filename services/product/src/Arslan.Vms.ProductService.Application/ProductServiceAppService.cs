using Arslan.Vms.ProductService.Localization;
using Volo.Abp.Application.Services;

namespace Arslan.Vms.ProductService;

public abstract class ProductServiceAppService : ApplicationService
{
    protected ProductServiceAppService()
    {
        LocalizationResource = typeof(ProductServiceResource);
        ObjectMapperContext = typeof(ProductServiceApplicationModule);
    }
}
