using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.IdentityService.Companies
{
    public class CompanyDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Company, Guid> _companyRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;

        public CompanyDataSeedContributor(
            IRepository<Company, Guid> companyRepository,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _companyRepository = companyRepository;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            var companies = await _companyRepository.GetListAsync();

            if (!companies.Any(a => a.TenantId == _currentTenant.Id))
            {
                //var company = new Company(_guidGenerator.Create(), _currentTenant.Id, null, _currentTenant.Name);
                //await _companyRepository.InsertAsync(company);
            }
        }
    }
}