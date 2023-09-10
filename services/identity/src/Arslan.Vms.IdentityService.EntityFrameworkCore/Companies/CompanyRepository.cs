using Arslan.Vms.IdentityService.Addresses;
using Arslan.Vms.IdentityService.EntityFrameworkCore;
using Arslan.Vms.IdentityService.Files;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.IdentityService.Companies
{
    public class CompanyRepository : EfCoreRepository<IIdentityServiceDbContext, Company, Guid>, ICompanyRepository
    {
        public CompanyRepository(IDbContextProvider<IIdentityServiceDbContext> dbContextProvider) : base(dbContextProvider)
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