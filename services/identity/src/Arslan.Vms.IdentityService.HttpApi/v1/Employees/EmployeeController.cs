using Arslan.Vms.IdentityService.v1.Employee;
using Arslan.Vms.IdentityService.v1.Employee.Dtos;
using Arslan.Vms.IdentityService.v1.Users.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.IdentityService.v1.Employees
{
    [ApiController]
    //[RemoteService(Name = "ArslanVmsIdentity")]
    [Area("Base")]
    [ControllerName("Employee")]
    [Route("rest/api/latest/vms/base/employee")]
    //[ApiVersion("1.0")]
    public class IdentityEmployeeController : AdministrationServiceController, IEmployeeAppService
    {
        protected IEmployeeAppService _employeeAppService { get; }
        IAuthorizationService _authorizationService { get; }

        public IdentityEmployeeController(IEmployeeAppService employeeAppService, IAuthorizationService authorizationService)
        {
            _employeeAppService = employeeAppService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        public Task<EmployeeDto> CreateAsync(CreateEmployeeDto input)
        {
            ValidateModel();
            return _employeeAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto input)
        {
            return _employeeAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public virtual Task DeleteAsync([FromForm(Name = "key")] string key)
        {
            return _employeeAppService.DeleteAsync(key);
        }

        [HttpPost]
        [Route("Undo")]
        public virtual Task UndoAsync(Guid id)
        {
            return _employeeAppService.UndoAsync(id);
        }

        [HttpGet]
        [Route("{id}/{isDeleted}")]
        public virtual Task<EmployeeDto> GetAsync(Guid id, bool isDeleted = false)
        {
            return _employeeAppService.GetAsync(id, isDeleted);
        }

        [HttpGet]
        public virtual Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            return _employeeAppService.GetListAsync(loadOptions);
        }

        [HttpGet]
        [Route("Employee")]
        public Task<RoleUserDto> GetEmployee(Guid id)
        {
            return _employeeAppService.GetEmployee(id);
        }

        [HttpGet]
        [Route("Technicians")]
        public virtual Task<List<RoleUserDto>> GetTechnicians()
        {
            return _employeeAppService.GetTechnicians();
        }

        [HttpGet]
        [Route("HeadTechnicians")]
        public virtual Task<List<RoleUserDto>> GetHeadTechnicians()
        {
            return _employeeAppService.GetHeadTechnicians();
        }

        [HttpGet]
        [Route("EmployeeRoles")]
        public virtual Task<List<RoleDto>> GetRoles()
        {
            return _employeeAppService.GetRoles();
        }
    }
}