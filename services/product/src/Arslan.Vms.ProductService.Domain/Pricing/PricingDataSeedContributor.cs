using Arslan.Vms.ProductService.Currencies;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.ProductService.Pricing
{
    public class PricingDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Currency, Guid> _currencyRepository;
        private readonly IRepository<PricingScheme, Guid> _pricingSchemeRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;

        public PricingDataSeedContributor(
            IRepository<Currency, Guid> currencyRepository,
            IRepository<PricingScheme, Guid> pricingSchemeRepository,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
            _currencyRepository = currencyRepository;
            _pricingSchemeRepository = pricingSchemeRepository;
            _currentTenant = currentTenant;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            var currencyList = _currencyRepository.GetListAsync().Result;
            if (!currencyList.Select(s => s.Code).Contains("TR"))
            {
                var data = new Currency(_guidGenerator.Create(), "TR", "Turkish New Lira  (TL)", "TL", 2, ",", ".", 2, 1);
                await _currencyRepository.InsertAsync(data);
                currencyList.Add(data);
            }
            if (!currencyList.Select(s => s.Code).Contains("USD"))
            {
                var data = new Currency(_guidGenerator.Create(), "USD", "US Dollar  ($)", "$", 2, ".", ",", 1, 3);
                await _currencyRepository.InsertAsync(data);
                currencyList.Add(data);
            }
            if (!currencyList.Select(s => s.Code).Contains("EUR"))
            {
                var data = new Currency(_guidGenerator.Create(), "EUR", "EU Euro  (€)", "€", 2, ".", ",", 2, 1);
                await _currencyRepository.InsertAsync(data);
                currencyList.Add(data);
            }

            var pricingSheme = _pricingSchemeRepository.GetListAsync().Result.Select(s => s.Name);
            if (!pricingSheme.Contains("Normal Price"))
            {
                var data = new PricingScheme(_guidGenerator.Create(), _currentTenant.Id, "Normal Price", currencyList.FirstOrDefault().Id, false);
                await _pricingSchemeRepository.InsertAsync(data);
            }
        }
    }
}