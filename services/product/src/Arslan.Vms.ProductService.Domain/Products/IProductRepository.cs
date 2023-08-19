using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.ProductService.Products
{
    public interface IProductRepository : IRepository<Product, Guid>
    {
        Task<Product> GetAsNoTracking(Guid id);
    }
}