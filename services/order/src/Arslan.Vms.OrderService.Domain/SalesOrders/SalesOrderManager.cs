using Arslan.Vms.OrderService.SalesOrders.Versions;
using Arslan.Vms.OrderService.Taxes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.OrderService.SalesOrders
{
    public class SalesOrderManager : DomainService
    {

        #region Fields
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IRepository<SalesOrderVersion, Guid> _salesOrderVersionRepository;
        private readonly IRepository<SalesOrderLineVersion, Guid> _salesOrderLineVersionRepository;
        private readonly IRepository<SalesOrderPickLineVersion, Guid> _salesOrderPickLineVersionRepository;
        private readonly SalesOrderTransactionManager _transactionManager;
        private readonly IRepository<TaxingScheme, Guid> _taxingSchemeRepository;
        private readonly IDataFilter _dataFilter;
        private readonly IObjectMapper _objectMapper;
        #endregion

        #region Ctor
        public SalesOrderManager(ISalesOrderRepository salesOrderRepository,
                                 IRepository<SalesOrderVersion, Guid> purchaseOrderVersionRepository,
                                 IRepository<TaxingScheme, Guid> taxingSchemeRepository,
                                 IRepository<SalesOrderLineVersion, Guid> purchaseOrderLineVersionRepository,
                                 IRepository<SalesOrderPickLineVersion, Guid> purchaseOrderPickLineVersionRepository,
                                 IDataFilter dataFilter,
                                 IObjectMapper objectMapper,
                                 SalesOrderTransactionManager transactionManager)
        {
            _salesOrderRepository = salesOrderRepository;
            _salesOrderVersionRepository = purchaseOrderVersionRepository;
            _salesOrderLineVersionRepository = purchaseOrderLineVersionRepository;
            _salesOrderPickLineVersionRepository = purchaseOrderPickLineVersionRepository;
            _taxingSchemeRepository = taxingSchemeRepository;
            _dataFilter = dataFilter;
            _transactionManager = transactionManager;
            _objectMapper = objectMapper;
        }
        #endregion

        public async Task CreateAsync(SalesOrder so)
        {
            var tax = _taxingSchemeRepository.WithDetails(w => w.TaxCodes).FirstOrDefault(f => f.Id == so.TaxingSchemeId);
            so.SetBalance(so.AmountPaid, tax);

            await _salesOrderRepository.InsertAsync(so, true);

            //Stock Updated
            await _transactionManager.UpdateBalancesForSalesOrder(so.Id, true);
        }


        public async Task UpdateAsync(SalesOrder so)
        {
            await Version(so.Id);

            so.IncreaseVersion();


            var tax = _taxingSchemeRepository.WithDetails(w => w.TaxCodes).FirstOrDefault(f => f.Id == so.TaxingSchemeId);
            so.SetBalance(so.AmountPaid, tax);

            await _salesOrderRepository.UpdateAsync(so, true);

            //Stock Updated
            if (so.IsStockChanged())
            {
                await _transactionManager.UpdateBalancesForSalesOrder(so.Id, true);
            }
        }

        public async Task DeleteAsync(string key)
        {
            var so = await _salesOrderRepository.FirstOrDefaultAsync(f => f.Id == Guid.Parse(key));
            so.SetCancel(true);
            await _salesOrderRepository.DeleteAsync(so, true);

            //Stock Updated
            await _transactionManager.UpdateBalancesForSalesOrder(so.Id, true);
        }

        public async Task UndoAsync(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var so = await _salesOrderRepository.FirstOrDefaultAsync(f => f.Id == id);
                so.SetCancel(false);
                await _salesOrderRepository.UpdateAsync(so, true);
            }

            //Stock Updated
            using (_dataFilter.Enable<ISoftDelete>())
            {
                await _transactionManager.UpdateBalancesForSalesOrder(id, true);
            }
        }

        async Task Version(Guid id)
        {
            var selectSO = _salesOrderRepository.WithDetails().FirstOrDefault(f => f.Id == id);

            var oldSO = _objectMapper.Map<SalesOrder, SalesOrderVersion>(selectSO);

            foreach (var item in oldSO.Lines)
            {
                await _salesOrderLineVersionRepository.InsertAsync(item, true);
            }

            foreach (var item in oldSO.PickLines)
            {
                await _salesOrderPickLineVersionRepository.InsertAsync(item, true);
            }

            await _salesOrderVersionRepository.InsertAsync(oldSO);
        }
    }
}