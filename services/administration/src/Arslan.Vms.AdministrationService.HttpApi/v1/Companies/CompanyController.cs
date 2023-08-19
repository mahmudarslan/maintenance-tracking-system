using Arslan.Vms.AdministrationService.v1.Company;
using Arslan.Vms.AdministrationService.v1.Company.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Arslan.Vms.AdministrationService.v1.Companies
{
    [ApiController]
    //[RemoteService(Name = "ArslanVmsIdentity")]
    [Area("Base")]
    [ControllerName("Company")]
    [Route("rest/api/latest/vms/base/company")]
    //[ApiVersion("1.0")]
    public class IdentityCompanyController : AdministrationServiceController, ICompanyAppService
    {
        protected ICompanyAppService _companyAppService { get; }

        public IdentityCompanyController(ICompanyAppService companyAppService)
        {
            _companyAppService = companyAppService;
        }


        [HttpPut]
        public virtual Task<CompanyDto> UpdateAsync(CreateUpdateCompanyDto input)
        {
            return _companyAppService.UpdateAsync(input);
        }

        [HttpGet]
        public virtual Task<CompanyDto> GetAsync(Guid id)
        {
            return _companyAppService.GetAsync(id);
        }
    }
}