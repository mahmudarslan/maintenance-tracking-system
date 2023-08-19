using Arslan.Vms.AdministrationService.v1.Company.Dtos;
using System;
using System.Threading.Tasks;

namespace Arslan.Vms.AdministrationService.v1.Company
{
    public interface ICompanyAppService
    {
        Task<CompanyDto> GetAsync(Guid id);
        Task<CompanyDto> UpdateAsync(CreateUpdateCompanyDto input);
    }
}