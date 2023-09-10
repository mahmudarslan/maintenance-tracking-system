using Arslan.Vms.InventoryService.DocumentNoFormats;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.Inventory
{
    public class InventoryDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<InvDocNoFormat, Guid> _docNoFormatRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public InventoryDataSeedContributor(
            IRepository<InvDocNoFormat, Guid> docNoFormatRepository,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _docNoFormatRepository = docNoFormatRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await CreateDocFormats();
        }

        async Task CreateDocFormats()
        {
            var docNoFormatId = (_docNoFormatRepository.GetListAsync().Result).Select(s => (InvDocNoType)s.DocNoType);
            if (!docNoFormatId.Contains(InvDocNoType.StockAdjust)) await _docNoFormatRepository.InsertAsync(new InvDocNoFormat(_guidGenerator.Create(), 0, 6, "SA-", "", InvDocNoType.StockAdjust, _currentTenant.Id));
        }
    }
}