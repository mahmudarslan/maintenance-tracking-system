using Arslan.Vms.AdministrationService.Addresses;
using Arslan.Vms.AdministrationService.Files;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.AdministrationService.Companies
{
    public interface ICompanyRepository : IRepository<Company, Guid>
    {
        Task<List<Address>> GetAddresses(Guid companId);
        Task<List<FileAttachment>> GetAttachmets(Guid companId);
    }
}