using Arslan.Vms.IdentityService.v1.Company;
using Arslan.Vms.IdentityService.v1.Company.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Arslan.Vms.IdentityService.v1.Companies
{
	[ApiVersion("1.0")]
	[ApiController]
	[Area("Base")]
	[ControllerName("Company")]
	[Route("identity/v{version:apiVersion}/base/company")]
	public class IdentityCompanyController : IdentityServiceController, ICompanyAppService
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