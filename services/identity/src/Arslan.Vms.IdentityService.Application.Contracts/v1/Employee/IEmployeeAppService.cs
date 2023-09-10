using Arslan.Vms.IdentityService;
using Arslan.Vms.IdentityService.v1.Employee.Dtos;
using Arslan.Vms.IdentityService.v1.Users.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.IdentityService.v1.Employee
{
    public interface IEmployeeAppService
    {
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto input);
        Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto input);
        Task DeleteAsync(string key);
        Task UndoAsync(Guid id);
        Task<EmployeeDto> GetAsync(Guid id, bool isDeleted = false);
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
        Task<RoleUserDto> GetEmployee(Guid id);
        Task<List<RoleUserDto>> GetTechnicians();
        Task<List<RoleUserDto>> GetHeadTechnicians();
        Task<List<RoleDto>> GetRoles();
    }
}