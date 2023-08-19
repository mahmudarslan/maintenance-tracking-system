using Arslan.Vms.VehicleService.Addresses;
using Arslan.Vms.VehicleService.Files;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.VehicleService.Companies
{
    public interface ICompanyRepository : IRepository<Company, Guid>
    {
        Task<List<Address>> GetAddresses(Guid companId);
        Task<List<FileAttachment>> GetAttachmets(Guid companId);
    }
}