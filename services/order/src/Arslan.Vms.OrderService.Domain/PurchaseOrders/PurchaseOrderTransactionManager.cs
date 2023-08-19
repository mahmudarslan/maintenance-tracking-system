using Arslan.Vms.OrderService.Accounts;
using Arslan.Vms.OrderService.Accounts.AccountTypes;
using Arslan.Vms.OrderService.Accounts.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.PurchaseOrders
{
    public class PurchaseOrderTransactionManager : DomainService
    {
        #region Fields

        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly AccountManager _accountManager;
        private readonly ICurrentTenant _currentTenant;
        private readonly IDataFilter _dataFilter;

        #endregion

        #region Ctor
        public PurchaseOrderTransactionManager(
               ICurrentTenant currentTenant,
               AccountManager accountManager,
               IDataFilter dataFilter,
               IPurchaseOrderRepository purchaseOrderRepository

            )
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _accountManager = accountManager;
            _currentTenant = currentTenant;
            _dataFilter = dataFilter;
        }
        #endregion

        public async Task UpdateBalancesForPurchaseOrder(Guid purchaseOrderId, bool allowNegativeInventory)
        {
            if (_accountManager.SuspendAccountingCalculations)
            {
                return;
            }

            var oldTransactions = await GetLoggedTransactions(purchaseOrderId);
            var newTransactions = await GetNewTransactions(purchaseOrderId);
            await _accountManager.UpdateTransactions(oldTransactions, newTransactions, null, true, allowNegativeInventory);
        }

        async Task<List<ITransaction>> GetLoggedTransactions(Guid purchaseOrderId)
        {
            var transactions = new List<ITransaction>();

            transactions.Add(await _accountManager.GetLoggedTransaction(new TransactionId((int)OrderTransactionType.PurchaseOrder, purchaseOrderId)));
            transactions.Add(await _accountManager.GetLoggedTransaction(new TransactionId((int)OrderTransactionType.PurchaseOrderServiceDone, purchaseOrderId)));
            transactions.AddRange(await _accountManager.GetLoggedTransactionGroup((int)OrderTransactionType.PurchaseOrderReceive, purchaseOrderId));
            //transactions.Add(await _accountManager.GetLoggedTransaction(new TransactionId((int)OrderTransactionType.PurchaseOrderInvoice, purchaseOrderId)));
            //transactions.AddRange(await _accountManager.GetLoggedTransactionGroup((int)OrderTransactionType.PurchaseOrderReturn, purchaseOrderId));
            //transactions.AddRange(await _accountManager.GetLoggedTransactionGroup((int)OrderTransactionType.PurchaseOrderUnstock, purchaseOrderId));
            //transactions.AddRange(await _accountManager.GetLoggedTransactionGroup((int)OrderTransactionType.PurchaseOrderPayments, purchaseOrderId));

            return transactions.Where(w => w != null).ToList();
        }

        public async Task<List<ITransaction>> GetNewTransactions(Guid purchaseOrderId)
        {
            var po = await _purchaseOrderRepository.GetWithDeatilsAsync(purchaseOrderId);

            var transactions = new List<ITransaction>();

            if (po != null && !po.IsDeleted)
            {
                transactions.Add(await GetPurchaseOrder(po));
                transactions.Add(await GetPurchaseOrderServiceDone(po));
                transactions.AddRange(await GetPurchaseOrderReceive(po));
                //transactions.Add(await GetPurchaseOrderInvoice(po));
                //transactions.AddRange(await GetPurchaseOrderReturn(po));
                //transactions.AddRange(await GetPurchaseOrderUnstock(po));
                //transactions.AddRange(await GetPurchaseOrderPayments(po));
            }

            var notNullTransactions = transactions.Where(w => w != null).ToList();

            await _accountManager.LoadTransactionAccountIds(notNullTransactions);

            return notNullTransactions;
        }

        async Task<Transaction> GetPurchaseOrder(PurchaseOrder po)
        {
            if (po.IsDeleted)
            {
                return null;
            }

            var entries = new List<Entry>();

            entries.AddRange(
                from s in po.OrderLines
                select new Entry(new AccountInvPurchased(s.ProductId, po.VendorId), -s.Quantity));

            entries.AddRange(
                from s in po.OrderLines
                select new Entry(new AccountInvOnOrder(s.ProductId), s.Quantity));

            entries.AddRange(po.OrderLines.Where((x) =>
            {
                bool? serviceCompleted = x.ServiceCompleted;
                if (!serviceCompleted.GetValueOrDefault())
                {
                    return false;
                }
                return serviceCompleted.HasValue;
            }).Select((l) => new Entry(new AccountMnyCOGS(l.ProductId), decimal.Zero)));

            entries.AddRange(po.OrderLines.Where((x) =>
            {
                bool? serviceCompleted = x.ServiceCompleted;
                if (!serviceCompleted.GetValueOrDefault())
                {
                    return false;
                }
                return serviceCompleted.HasValue;
            }).Select((l) => new Entry(new AccountMnyServiceCosts(l.ProductId), decimal.Zero)));

            var ss = new Transaction(po.PurchaseOrderTransactionId, po.OrderDate, entries, true);

            return await Task.FromResult(ss);
        }

        async Task<Transaction> GetPurchaseOrderInvoice(PurchaseOrder po)
        {
            //var transactions = new Transaction();

            return null;
        }

        async Task<List<Transaction>> GetPurchaseOrderPayments(PurchaseOrder po)
        {
            var transactions = new List<Transaction>();

            return await Task.FromResult(transactions);
        }

        async Task<List<Transaction>> GetPurchaseOrderReceive(PurchaseOrder po)
        {
            var transactions = new List<Transaction>();

            if (po.IsDeleted)
            {
                return await Task.FromResult(transactions.Select(s => s).ToList());
            }

            foreach (PurchaseOrderReceiveLine poReceiveLine in po.ReceiveLines.Where(w => w.Quantity != decimal.Zero))
            {
                var entries = new List<Entry>();

                //var itemType = poReceiveLine.Product.ProductType;

                //if (itemType == ProductType.StockedProduct)
                //{
                //    entries.Add(new Entry(new AccountInvOnOrder(poReceiveLine.ProductId), -poReceiveLine.Quantity));

                //    var onhHands = new Entry(new AccountInvOnHand(poReceiveLine.ProductId, poReceiveLine.LocationId, poReceiveLine.Sublocation), poReceiveLine.Quantity);

                //    entries.Add(onhHands);
                //}
                //else if (itemType == ProductType.NonStockedProduct)
                //{
                //    entries.Add(new Entry(new AccountInvNonstockedReceived(poReceiveLine.ProductId), poReceiveLine.Quantity));
                //    entries.Add(new Entry(new AccountInvOnOrder(poReceiveLine.ProductId), -poReceiveLine.Quantity));
                //}

                transactions.Add(new Transaction(new TransactionId((int)OrderTransactionType.PurchaseOrderReceive, po.Id, poReceiveLine.Id), poReceiveLine.ReceiveDate, entries, true));
            }

            var ss = transactions.Select(s => s).ToList();

            return await Task.FromResult(ss);
        }

        async Task<List<Transaction>> GetPurchaseOrderReturn(PurchaseOrder po)
        {
            var transactions = new List<Transaction>();

            return await Task.FromResult(transactions);
        }

        async Task<Transaction> GetPurchaseOrderServiceDone(PurchaseOrder po)
        {
            if (po.IsDeleted)
            {
                return null;
            }

            PurchaseOrderLine[] array = po.OrderLines.Where((x) =>
            {
                bool? serviceCompleted = x.ServiceCompleted;
                if (!serviceCompleted.GetValueOrDefault())
                {
                    return false;
                }
                return serviceCompleted.HasValue;
            }).ToArray();

            if (array.Length == 0)
            {
                return null;
            }

            var entries = new List<Entry>();

            entries.AddRange(
                from s in array
                select new Entry(new AccountInvOnOrder(s.ProductId), -s.Quantity));

            entries.AddRange(
                from s in array
                select new Entry(new AccountInvServicesReceived(s.ProductId), s.Quantity));

            entries.AddRange(
                from l in array
                select new Entry(new AccountMnyCOGS(l.ProductId), decimal.Zero));

            entries.AddRange(
                from l in array
                select new Entry(new AccountMnyServiceCosts(l.ProductId), decimal.Zero));

            //return await Task.FromResult(new Transaction(so.PurchaseOrderServiceDoneTransactionId, so.OrderDate, entries, true));

            //var transactions = new Transaction();

            return null;
        }

        async Task<List<Transaction>> GetPurchaseOrderUnstock(PurchaseOrder po)
        {
            var transactions = new List<Transaction>();

            return await Task.FromResult(transactions);
        }

    }
}