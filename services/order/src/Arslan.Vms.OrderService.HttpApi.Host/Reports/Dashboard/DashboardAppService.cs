using Arslan.Vms.OrderService.SalesOrders;
using Arslan.Vms.OrderService.v1.Reports.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.v1.Reports.Dashboard
{
    [Authorize]
    public class DashboardAppService : OrderServiceAppService
    {
        private readonly IRepository<SalesOrder, Guid> _salesOrderRepository;
        //private readonly IRepository<Product, Guid> _toolRepository;
        //private readonly IUserRepository _userRepository;
        private readonly ICurrentTenant _currentTenant;
        //private readonly IRepository<UserRole> _userRoleRepository;
        //private IRepository<Role, Guid> _roleRepository;

        public DashboardAppService(
       IRepository<SalesOrder, Guid> salesOrderRepository,
       //IRepository<Product, Guid> toolRepository,
       //IUserRepository userRepository,
       ICurrentTenant currentTenant
       //IRepository<UserRole> userRoleRepository,
       //IRepository<Role, Guid> roleRepository
       )
        {
            _salesOrderRepository = salesOrderRepository;
            //_toolRepository = toolRepository;
            //_userRepository = userRepository;
            _currentTenant = currentTenant;
            //_userRoleRepository = userRoleRepository;
            //_roleRepository = roleRepository;
        }

        [HttpGet]
        public async Task<DashboardDto> Get()
        {
            var result = new DashboardDto();

            //var status = _salesOrderRepository.Select(s => s.SalesOrderStatus).ToList();
            //result.Quote = status.Where(w => w == SalesOrderStatus.Quote).Count();
            //result.Open = status.Where(w => w == SalesOrderStatus.Open).Count();
            //result.InProgress = status.Where(w => w == SalesOrderStatus.InProgress).Count();
            //result.Invoiced = status.Where(w => w == SalesOrderStatus.Invoiced).Count();
            //result.Paid = status.Where(w => w == SalesOrderStatus.Paid).Count();
            //result.Cancelled = status.Where(w => w == SalesOrderStatus.Cancelled).Count();
            return await Task.FromResult(result);
        }

        [HttpGet]
        public async Task<TenantAbilityStepGuide> GetTenantAbilityStepGuide()
        {
            //var roleRepository = await _roleRepository.GetQueryableAsync();
            //var userRoleRepository = await _userRoleRepository.GetQueryableAsync();

            var result = new TenantAbilityStepGuide()
            {
                IsCustomerDefined = false,
                IsHeadTechnicianDefined = false,
                IsTechicianDefined = false
            };

            //result.IsCustomerDefined = _userVehicleRepository.Any(w => w.TenantId == _currentTenant.Id);

            //bool hasAnyUser = await _userRepository.AnyAsync(w => w.TenantId == _currentTenant.Id);

            //if (hasAnyUser)
            //{
            //    var selectedTechnicianRole = from role in roleRepository
            //                                 from userRole in userRoleRepository
            //                                 where role.Id == userRole.RoleId
            //                                 select role
            //                                ;
            //    result.IsTechicianDefined = selectedTechnicianRole.Any(w => w.NormalizedName == RoleConsts.Technician);
            //    result.IsHeadTechnicianDefined = selectedTechnicianRole.Any(w => w.NormalizedName == RoleConsts.HeadTechnician);
            //}
            //else
            //{
            //    result.IsTechicianDefined = false;
            //    result.IsHeadTechnicianDefined = false;
            //}

            return await Task.FromResult(result);
        }
    }
}