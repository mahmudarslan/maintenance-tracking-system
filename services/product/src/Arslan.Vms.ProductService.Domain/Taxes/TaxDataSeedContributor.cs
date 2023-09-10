using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.ProductService.Taxes
{
    public class TaxDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<TaxingScheme, Guid> _taxingSchemeRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;

        public TaxDataSeedContributor(
            IRepository<TaxingScheme, Guid> taxingSchemeRepository,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
            _taxingSchemeRepository = taxingSchemeRepository;
            _currentTenant = currentTenant;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            var taxingScheme = _taxingSchemeRepository.GetListAsync().Result.Select(s => s.Name);
            if (!taxingScheme.Contains("No Tax %0"))
            {
                var data = new TaxingScheme(_guidGenerator.Create(), _currentTenant.Id, "No Tax %0", "", "", false);
                data.AddTaxCode(_guidGenerator.Create(), "Taxable", 0, 0);
                data.AddTaxCode(_guidGenerator.Create(), "Non-Taxable", 0, 0);
                data.SetDefaultTaxCode(data.TaxCodes[0].Id);
                await _taxingSchemeRepository.InsertAsync(data);
            }
            if (!taxingScheme.Contains("TC Tax"))
            {
                var data = new TaxingScheme(_guidGenerator.Create(), _currentTenant.Id, "TC Tax", "KDV %18", "ÖTV %20", false);
                data.AddTaxCode(_guidGenerator.Create(), "Taxable", 18, 20);
                data.AddTaxCode(_guidGenerator.Create(), "Non-Taxable", 0, 0);
                data.SetDefaultTaxCode(data.TaxCodes[0].Id);
                await _taxingSchemeRepository.InsertAsync(data);
            }
        }
    }
}