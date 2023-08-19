using Arslan.Vms.OrderService.Accounts;
using Arslan.Vms.OrderService.Accounts.AccountTypes;
using Arslan.Vms.OrderService.Accounts.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.SalesOrders
{
    public class SalesOrderTransactionManager : DomainService
    {
        #region Fields

        private readonly ICurrentTenant _currentTenant;
        private readonly AccountManager _accountManager;
        private readonly ISalesOrderRepository _salesOrderRepository;

        #endregion

        #region Ctor
        public SalesOrderTransactionManager(
               ICurrentTenant currentTenant,
               AccountManager accountManager,
               ISalesOrderRepository salesOrderRepository

            )
        {
            _salesOrderRepository = salesOrderRepository;
            _accountManager = accountManager;
            _currentTenant = currentTenant;
        }
        #endregion

        public async Task UpdateBalancesForSalesOrder(Guid salesOrderId, bool allowNegativeInventory)
        {
            if (_accountManager.SuspendAccountingCalculations)
            {
                return;
            }

            var oldTransactions = await GetLoggedTransactions(salesOrderId);
            var newTransactions = await GetNewTransactions(salesOrderId);
            await _accountManager.UpdateTransactions(oldTransactions, newTransactions, null, true, allowNegativeInventory);
        }

        async Task<List<ITransaction>> GetLoggedTransactions(Guid salesOrderId)
        {
            var transactions = new List<ITransaction>();

            transactions.Add(await _accountManager.GetLoggedTransaction(new TransactionId((int)OrderTransactionType.SalesOrder, salesOrderId)));
            transactions.Add(await _accountManager.GetLoggedTransaction(new TransactionId((int)OrderTransactionType.SalesOrderServiceDone, salesOrderId)));
            transactions.AddRange(await _accountManager.GetLoggedTransactionGroup((int)OrderTransactionType.SalesOrderFulfillment, salesOrderId));
            transactions.AddRange(await _accountManager.GetLoggedTransactionGroup((int)OrderTransactionType.SalesOrderPayments, salesOrderId));

            return transactions.Where(w => w != null).ToList();
        }

        public async Task<List<ITransaction>> GetNewTransactions(Guid salesOrderId)
        {
            var so = await _salesOrderRepository.GetWithDeatilsAsync(salesOrderId);

            var transactions = new List<ITransaction>();
            if (so != null && !so.IsQuote)
            {
                transactions.Add(await GetSalesOrder(so));
                transactions.Add(await GetSalesOrderServiceDone(so));
                transactions.AddRange(await GetSalesOrderFulfillment(so));
                transactions.AddRange(await GetSalesOrderPayments(so));
            }

            var notNullTransactions = transactions.Where(w => w != null).ToList();

            await _accountManager.LoadTransactionAccountIds(notNullTransactions);

            return notNullTransactions;
        }


        async Task<Transaction> GetSalesOrder(SalesOrder so)
        {
            if (so.IsQuote)
            {
                return null;
            }

            var entries = new List<Entry>();

            entries.AddRange(
                from s in so.OrderLines
                select new Entry(new AccountInvPending(s.ProductId), -s.Quantity));

            entries.AddRange(
                from s in so.OrderLines
                select new Entry(new AccountInvSold(s.ProductId, so.CustomerId), s.Quantity));

            entries.AddRange(so.OrderLines.Where(x => x.ServiceCompleted == true).Select(s => new Entry(new AccountMnyCOGS(s.ProductId), 0)));

            entries.AddRange(so.OrderLines.Where(x => x.ServiceCompleted == true).Select(s => new Entry(new AccountMnyServiceCosts(s.ProductId), 0)));

            return await Task.FromResult(new Transaction(so.SalesOrderTransactionId, so.OrderDate, entries, true));
        }

        async Task<List<Transaction>> GetSalesOrderFulfillment(SalesOrder so)
        {
            var transactions = new List<Transaction>();

            foreach (SalesOrderPickLine salesOrderPickLine in so.PickLines.Where(w => w.Quantity != 0))
            {
                var entries = new List<Entry>();

                //var itemType = salesOrderPickLine.Product.ProductType;

                //if (itemType == ProductType.StockedProduct)
                //{
                //    entries.Add(new Entry(new AccountInvOnHand(salesOrderPickLine.Product.Id, salesOrderPickLine.LocationId, salesOrderPickLine.Sublocation), -salesOrderPickLine.Quantity));
                //    entries.Add(new Entry(new AccountInvTotalOnHand(salesOrderPickLine.Product.Id), -salesOrderPickLine.Quantity));
                //    entries.Add(new Entry(new AccountInvTotalOwned(salesOrderPickLine.Product.Id), -salesOrderPickLine.Quantity));
                //    entries.Add(new Entry(new AccountMnyInventoryValue(salesOrderPickLine.ProductId), 0));
                //}
                //else if (itemType == ProductType.NonStockedProduct)
                //{
                //    entries.Add(new Entry(new AccountInvNonstockedUsed(salesOrderPickLine.ProductId), -salesOrderPickLine.Quantity));
                //    entries.Add(new Entry(new AccountMnyNonstockedCosts(salesOrderPickLine.ProductId), 0));
                //}
                entries.Add(new Entry(new AccountMnyCOGS(salesOrderPickLine.ProductId), 0));
                entries.Add(new Entry(new AccountInvPending(salesOrderPickLine.ProductId), salesOrderPickLine.Quantity));

                transactions.Add(new Transaction(so.SalesOrderFulfillmentTransactionId(salesOrderPickLine.Id), salesOrderPickLine.PickDate, entries, true));
            }

            return await Task.FromResult(transactions);
        }

        async Task<Transaction> GetSalesOrderServiceDone(SalesOrder so)
        {
            if (so.IsQuote)
            {
                return null;
            }

            var array = so.OrderLines.Where(x => x.ServiceCompleted == true).ToArray();

            if (array.Length == 0)
            {
                return null;
            }

            var entries = new List<Entry>();

            entries.AddRange(from s in array select new Entry(new AccountInvServicesPerformed(s.ProductId), -s.Quantity));
            entries.AddRange(from s in array select new Entry(new AccountInvPending(s.ProductId), s.Quantity));
            entries.AddRange(from s in array select new Entry(new AccountMnyCOGS(s.ProductId), 0));
            entries.AddRange(from s in array select new Entry(new AccountMnyServiceCosts(s.ProductId), 0));

            return await Task.FromResult(new Transaction(so.SalesOrderServiceDoneTransactionId, so.OrderDate, entries, true));
        }

        async Task<List<Transaction>> GetSalesOrderPayments(SalesOrder so)
        {
            var transactions = new List<Transaction>();

            return await Task.FromResult(transactions);
        }

    }
}