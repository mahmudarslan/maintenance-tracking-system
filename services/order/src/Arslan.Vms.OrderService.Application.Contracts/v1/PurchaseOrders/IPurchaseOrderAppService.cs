using Arslan.Vms.OrderService.v1.Dtos;
using Arslan.Vms.OrderService.v1.PurchaseOrders.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.OrderService.v1.PurchaseOrders
{
    public interface IPurchaseOrderAppService
    {
        Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto input);
        Task<PurchaseOrderDto> UpdateAsync(Guid id, UpdatePurchaseOrderDto input);
        Task DeleteAsync(string key);
        Task UndoAsync(Guid id);
        Task<PurchaseOrderDto> GetAsync(Guid id, bool isDeleted = false);
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
        Task<List<StatusDto>> GetStatusListAsync();
        Task<List<StatusDto>> GetInventoryStatusListAsync();
        Task<List<StatusDto>> GetPaymentStatusListAsync();
    }
}