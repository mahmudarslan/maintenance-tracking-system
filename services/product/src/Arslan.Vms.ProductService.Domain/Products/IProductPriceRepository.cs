using System;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.ProductService.Products
{
    public interface IProductPriceRepository : IRepository<ProductPrice, Guid>
    {
    }
}