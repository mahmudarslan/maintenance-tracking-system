using Arslan.Vms.OrderService.DocumentNoFormats;
using Arslan.Vms.OrderService.SalesOrders;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.OrderService
{
    public class OrderDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<OrderDocNoFormat, Guid> _docNoFormatRepository;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IRepository<WorkOrderType, Guid> _workorderTypeRepository;

        public OrderDataSeedContributor(
            IRepository<OrderDocNoFormat, Guid> docNoFormatRepository,
             IRepository<WorkOrderType, Guid> workorderTypeRepository,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _guidGenerator = guidGenerator;
            _docNoFormatRepository = docNoFormatRepository;
            _workorderTypeRepository = workorderTypeRepository;
            _currentTenant = currentTenant;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            var docNoFormatId = _docNoFormatRepository.GetListAsync().Result.Select(s => s.DocNoType);
            if (!docNoFormatId.Contains((int)OrderDocNoType.SalesOrder)) await _docNoFormatRepository.InsertAsync(new OrderDocNoFormat(_guidGenerator.Create(), 0, 6, "SO-", "", OrderDocNoType.SalesOrder, _currentTenant.Id));
            if (!docNoFormatId.Contains((int)OrderDocNoType.SalesQuote)) await _docNoFormatRepository.InsertAsync(new OrderDocNoFormat(_guidGenerator.Create(), 0, 6, "SQ-", "", OrderDocNoType.SalesQuote, _currentTenant.Id));
            if (!docNoFormatId.Contains((int)OrderDocNoType.PurchaseOrder)) await _docNoFormatRepository.InsertAsync(new OrderDocNoFormat(_guidGenerator.Create(), 0, 6, "PO-", "", OrderDocNoType.PurchaseOrder, _currentTenant.Id));
            await CreateWorkorderTypeRepository();
        }

        private async Task CreateWorkorderTypeRepository()
        {
            var data = _workorderTypeRepository.GetListAsync().Result.Select(s => s.Name);

            if (!data.Contains("Bakım")) await _workorderTypeRepository.InsertAsync(new WorkOrderType(_guidGenerator.Create(), "Bakım"));
            if (!data.Contains("Arıza")) await _workorderTypeRepository.InsertAsync(new WorkOrderType(_guidGenerator.Create(), "Arıza"));
            if (!data.Contains("Periyodik Bakım")) await _workorderTypeRepository.InsertAsync(new WorkOrderType(_guidGenerator.Create(), "Periyodik Bakım"));
            if (!data.Contains("Ağır Bakım")) await _workorderTypeRepository.InsertAsync(new WorkOrderType(_guidGenerator.Create(), "Ağır Bakım"));
            if (!data.Contains("Kaza")) await _workorderTypeRepository.InsertAsync(new WorkOrderType(_guidGenerator.Create(), "Kaza"));
        }
    }
}