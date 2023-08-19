using Arslan.Vms.ProductService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Arslan.Vms.ProductService;

public abstract class ProductServiceController : AbpControllerBase
{
    protected ProductServiceController()
    {
        LocalizationResource = typeof(ProductServiceResource);
    }
}
