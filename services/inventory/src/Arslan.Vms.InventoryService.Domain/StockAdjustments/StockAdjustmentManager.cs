using Arslan.Vms.InventoryService.DocumentNoFormats;
using Arslan.Vms.InventoryService.Localization;
using Arslan.Vms.InventoryService.StockAdjustments.Versions;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.InventoryService.StockAdjustments
{
    public class StockAdjustmentManager : DomainService
    {

        #region Fields
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
        private readonly IRepository<StockAdjustmentVersion, Guid> _stockAdjustmentVersionRepository;
        private readonly IRepository<StockAdjustmentLineVersion, Guid> _stockAdjustmentLineVersionRepository;
        private readonly StockAdjustmentTransactionManager _transactionManager;
        private readonly DocNoFormatManager _docNoFormatManager;
        private readonly IStringLocalizer<InventoryServiceResource> _localizer;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDataFilter _dataFilter;
        private readonly IObjectMapper _objectMapper;
        #endregion

        #region Ctor
        public StockAdjustmentManager(
     IStockAdjustmentRepository salesOrderRepository,
     IRepository<StockAdjustmentVersion, Guid> purchaseOrderVersionRepository,
     IRepository<StockAdjustmentLineVersion, Guid> purchaseOrderLineVersionRepository,
     ICurrentTenant currentTenant,
     IDataFilter dataFilter,
     IGuidGenerator guidGenerator,
     IObjectMapper objectMapper,
     IStringLocalizer<InventoryServiceResource> localizer,
     DocNoFormatManager docNoFormatManager,
     StockAdjustmentTransactionManager transactionManager
    )
        {
            _stockAdjustmentRepository = salesOrderRepository;
            _stockAdjustmentVersionRepository = purchaseOrderVersionRepository;
            _stockAdjustmentLineVersionRepository = purchaseOrderLineVersionRepository;
            _docNoFormatManager = docNoFormatManager;
            _currentTenant = currentTenant;
            _dataFilter = dataFilter;
            _guidGenerator = guidGenerator;
            _localizer = localizer;
            _transactionManager = transactionManager;
            _objectMapper = objectMapper;
        }
        #endregion

        public async Task CreateAsync(StockAdjustment sa)
        {
            await _stockAdjustmentRepository.InsertAsync(sa, true);

            //Stock Updated
            await _transactionManager.UpdateBalancesForStockAdjustment(sa.Id, true);
        }


        public async Task UpdateAsync(StockAdjustment so)
        {
            so.IncreaseVersion();

            await Version(so.Id);
            await _stockAdjustmentRepository.UpdateAsync(so, true);

            //Stock Updated
            if (so.IsStockChanged())
            {
                await _transactionManager.UpdateBalancesForStockAdjustment(so.Id, true);
            }
        }

        public async Task DeleteAsync(string key)
        {
            var so = await _stockAdjustmentRepository.FirstOrDefaultAsync(f => f.Id == Guid.Parse(key));
            so.SetCancel(true);
            await _stockAdjustmentRepository.DeleteAsync(so, true);

            //Stock Updated
            await _transactionManager.UpdateBalancesForStockAdjustment(so.Id, true);
        }

        public async Task UndoAsync(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var so = await _stockAdjustmentRepository.FirstOrDefaultAsync(f => f.Id == id);
                so.SetCancel(false);
                await _stockAdjustmentRepository.UpdateAsync(so, true);
            }

            //Stock Updated
            using (_dataFilter.Enable<ISoftDelete>())
            {
                await _transactionManager.UpdateBalancesForStockAdjustment(id, true);
            }
        }

        async Task Version(Guid id)
        {
            var selectSO = _stockAdjustmentRepository.WithDetails().FirstOrDefault(f => f.Id == id);

            var oldSO = _objectMapper.Map<StockAdjustment, StockAdjustmentVersion>(selectSO);

            foreach (var item in oldSO.Lines)
            {
                await _stockAdjustmentLineVersionRepository.InsertAsync(item, true);
            }

            await _stockAdjustmentVersionRepository.InsertAsync(oldSO);
        }
    }
}