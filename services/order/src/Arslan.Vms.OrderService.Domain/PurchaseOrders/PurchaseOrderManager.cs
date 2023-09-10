using Arslan.Vms.OrderService.DocumentNoFormats;
using Arslan.Vms.OrderService.Localization;
using Arslan.Vms.OrderService.PurchaseOrders.Versions;
using Arslan.Vms.OrderService.Taxes;
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

namespace Arslan.Vms.OrderService.PurchaseOrders
{
    public class PurchaseOrderManager : DomainService
    {

        #region Fields
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IRepository<PurchaseOrderVersion, Guid> _purchaseOrderVersionRepository;
        private readonly IRepository<PurchaseOrderLineVersion, Guid> _purchaseOrderLineVersionRepository;
        private readonly IRepository<PurchaseOrderReceiveLineVersion, Guid> _purchaseOrderPickLineVersionRepository;
        private readonly IRepository<TaxingScheme, Guid> _taxingSchemeRepository;
        private readonly PurchaseOrderTransactionManager _transactionManager;
        private readonly DocNoFormatManager _docNoFormatManager;
        private readonly IStringLocalizer<OrderServiceResource> _localizer;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDataFilter _dataFilter;
        private readonly IObjectMapper _objectMapper;
        #endregion

        #region Ctor
        public PurchaseOrderManager(
     IPurchaseOrderRepository purchaseOrderRepository,
     IRepository<PurchaseOrderVersion, Guid> purchaseOrderVersionRepository,
     IRepository<PurchaseOrderLineVersion, Guid> purchaseOrderLineVersionRepository,
     IRepository<PurchaseOrderReceiveLineVersion, Guid> purchaseOrderPickLineVersionRepository,
     IRepository<TaxingScheme, Guid> taxingSchemeRepository,
     ICurrentTenant currentTenant,
     IDataFilter dataFilter,
     IGuidGenerator guidGenerator,
     IObjectMapper objectMapper,
     IStringLocalizer<OrderServiceResource> localizer,
     DocNoFormatManager docNoFormatManager,
     PurchaseOrderTransactionManager transactionManager
    )
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _purchaseOrderVersionRepository = purchaseOrderVersionRepository;
            _purchaseOrderLineVersionRepository = purchaseOrderLineVersionRepository;
            _purchaseOrderPickLineVersionRepository = purchaseOrderPickLineVersionRepository;
            _docNoFormatManager = docNoFormatManager;
            _currentTenant = currentTenant;
            _dataFilter = dataFilter;
            _guidGenerator = guidGenerator;
            _localizer = localizer;
            _transactionManager = transactionManager;
            _objectMapper = objectMapper;
            _taxingSchemeRepository = taxingSchemeRepository;
        }
        #endregion

        public async Task CreateAsync(PurchaseOrder po)
        {
            var tax = _taxingSchemeRepository.WithDetails(w => w.TaxCodes).FirstOrDefault(f => f.Id == po.TaxingSchemeId);
            po.SetBalance(po.AmountPaid, tax);

            await _purchaseOrderRepository.InsertAsync(po, true);

            //Stock Updated
            await _transactionManager.UpdateBalancesForPurchaseOrder(po.Id, true);
        }


        public async Task UpdateAsync(PurchaseOrder po)
        {
            po.IncreaseVersion();

            await Version(po.Id);

            var tax = _taxingSchemeRepository.WithDetails(w => w.TaxCodes).FirstOrDefault(f => f.Id == po.TaxingSchemeId);
            po.SetBalance(po.AmountPaid, tax);
            await _purchaseOrderRepository.UpdateAsync(po, true);

            //Stock Updated
            if (po.IsStockChanged())
            {
                await _transactionManager.UpdateBalancesForPurchaseOrder(po.Id, true);
            }
        }

        public async Task DeleteAsync(string key)
        {
            var po = await _purchaseOrderRepository.FirstOrDefaultAsync(f => f.Id == Guid.Parse(key));
            po.SetCancel(true);
            await _purchaseOrderRepository.DeleteAsync(po, true);

            //Stock Updated
            await _transactionManager.UpdateBalancesForPurchaseOrder(po.Id, true);
        }

        public async Task UndoAsync(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var po = await _purchaseOrderRepository.FirstOrDefaultAsync(f => f.Id == id);
                po.SetCancel(false);
                await _purchaseOrderRepository.UpdateAsync(po, true);
            }

            //Stock Updated
            using (_dataFilter.Enable<ISoftDelete>())
            {
                await _transactionManager.UpdateBalancesForPurchaseOrder(id, true);
            }
        }

        async Task Version(Guid id)
        {
            var selectPO = _purchaseOrderRepository.WithDetails().FirstOrDefault(f => f.Id == id);

            var oldPO = _objectMapper.Map<PurchaseOrder, PurchaseOrderVersion>(selectPO);

            foreach (var item in oldPO.OrderLines)
            {
                await _purchaseOrderLineVersionRepository.InsertAsync(item, true);
            }

            foreach (var item in oldPO.ReceiveLines)
            {
                await _purchaseOrderPickLineVersionRepository.InsertAsync(item, true);
            }

            await _purchaseOrderVersionRepository.InsertAsync(oldPO);
        }
    }
}