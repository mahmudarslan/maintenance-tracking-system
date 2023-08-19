using Arslan.Vms.OrderService.v1.Dtos;
using Arslan.Vms.OrderService.v1.SalesOrders.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.OrderService.v1.SalesOrders
{
    public interface ISalesOrderAppService
    {
        Task<SalesOrderDto> CreateAsync(CreateSalesOrderDto input);
        Task<SalesOrderDto> UpdateAsync(Guid id, UpdateSalesOrderDto input);
        Task DeleteAsync(string key);
        Task UndoAsync(Guid id);
        Task<SalesOrderDto> GetAsync(Guid id, bool isDeleted = false);
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
        Task<List<StatusDto>> GetStatusListAsync();
        Task<List<WorkStatusDto>> GetWorkorderTypesListAsync();
        Task<List<StatusDto>> GetInventoryStatusListAsync();
        Task<List<StatusDto>> GetPaymentStatusListAsync();
    }
}