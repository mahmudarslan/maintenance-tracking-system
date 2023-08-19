using Arslan.Vms.AdministrationService.Addresses;
using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Arslan.Vms.AdministrationService.Files;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.AdministrationService.Companies
{
    public class CompanyRepository : EfCoreRepository<IAdministrationServiceDbContext, Company, Guid>, ICompanyRepository
    {
        public CompanyRepository(IDbContextProvider<IAdministrationServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public override Task<Company> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return base.GetAsync(id, includeDetails, cancellationToken);
        }


        //public override IQueryable<Company> WithDetails()
        //{
        //    return GetQueryable().IncludeDetails();
        //}

        public virtual async Task<List<Address>> GetAddresses(Guid companId)
        {
            var data = from a in DbContext.Set<CompanyAddress>()
                        join b in DbContext.Set<Address>() on a.AddressId equals b.Id
                        where a.CompanyId == companId
                        select b;

            return await data.ToListAsync();
        }

        public virtual async Task<List<FileAttachment>> GetAttachmets(Guid companId)
        {
            var data = from a in DbContext.Set<CompanyAttachment>()
                        join b in DbContext.Set<FileAttachment>() on a.FileAttachmentId equals b.Id
                        where a.CompanyId == companId
                        select b;

            return await data.ToListAsync();
        }


    }
}