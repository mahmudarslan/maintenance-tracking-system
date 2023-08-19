using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.OrderService.PurchaseOrders
{
    public interface IPurchaseOrderRepository : IRepository<PurchaseOrder, Guid>
    {
        Task<PurchaseOrder> GetWithDeatilsAsync(Guid id);
    }
}