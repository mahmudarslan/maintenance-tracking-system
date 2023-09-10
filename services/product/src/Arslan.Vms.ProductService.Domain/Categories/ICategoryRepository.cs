using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.ProductService.Categories
{
    public interface ICategoryRepository : IRepository<Category, Guid>
    {
        Task AddRangeAsync(List<Category> entityList, CancellationToken cancellationToken = default);
        void UpdateRange(List<Category> entityList);
    }
}