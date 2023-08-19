using Arslan.Vms.AdministrationService.v1.Customers.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.AdministrationService.v1.Customers
{
    public interface ICustomerAppService
    {
        Task<CustomerDto> CreateAsync(CreateCustomerDto input);
        Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto input);
        Task DeleteAsync(string key);
        Task UndoAsync(Guid id);
        Task UndoVehcileAsync(Guid userId, Guid vehicleId);
        Task<CustomerDto> GetAsync(Guid id, bool isDeleted = false);
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
        Task<List<CustomerListDto>> GetAllAsync();
        Task<List<CustomerSearchDto>> GetBySearchContent(string nameSurnameGsmPlateNo);
        Task<CustomerVehicleSearchDto> GetByVehicleId(Guid userId, Guid vehicleId);
        Task<CustomerReportDto> GetCustomerDetails(Guid userId, Guid vehicleId);
        Task<List<CustomerVehiclehDto>> GetCustomerVehicles(Guid customerId);
        Task<CustomerVehiclehDto> GetCustomerVehicle(Guid userId, Guid vehicleId);
        Task<LoadResult> GetVehicleListAsync(DataSourceLoadOptions loadOptions);
    }
}