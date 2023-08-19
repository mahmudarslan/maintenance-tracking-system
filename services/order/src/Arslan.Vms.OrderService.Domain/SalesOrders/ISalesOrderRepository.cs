using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.OrderService.SalesOrders
{
    public interface ISalesOrderRepository : IRepository<SalesOrder, Guid>
    {
        Task<SalesOrder> GetWithDeatilsAsync(Guid id);
    }
}