//using Arslan.Vms.Base.Products;
using Arslan.Vms.InventoryService.Accounts;
using Arslan.Vms.InventoryService.Accounts.AccountTypes;
using Arslan.Vms.InventoryService.Accounts.TransactionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Arslan.Vms.InventoryService.StockAdjustments
{
    public class StockAdjustmentTransactionManager : DomainService
    {
        #region Fields
        private readonly IStockAdjustmentRepository _stockAdjustmentRepository;
        //private readonly IRepository<Product, Guid> _productRepository;
        private readonly AccountManager _accountManager;
        #endregion

        #region Ctor
        public StockAdjustmentTransactionManager(
               //IRepository<Product, Guid> productRepository,
               IStockAdjustmentRepository stockAdjustmentRepository,
               AccountManager accountManager)
        {
            _accountManager = accountManager;
            //_productRepository = productRepository;
            _stockAdjustmentRepository = stockAdjustmentRepository;
        }
        #endregion

        public async Task UpdateBalancesForStockAdjustment(Guid stockAdjustmentId, bool allowNegativeInventory)
        {
            if (_accountManager.SuspendAccountingCalculations)
            {
                return;
            }

            var loggedTransactionsForStockAdjustment = await GetLoggedTransactionsForStockAdjustment(stockAdjustmentId);
            var newStockAdjustmentTransactions = await GetNewStockAdjustmentTransactions(stockAdjustmentId);
            await _accountManager.UpdateTransactions(loggedTransactionsForStockAdjustment, newStockAdjustmentTransactions, null, true, allowNegativeInventory);
        }

        async Task<List<ITransaction>> GetLoggedTransactionsForStockAdjustment(Guid stockAdjustmentId)
        {
            return await Task.FromResult((
                from x in new ITransaction[] { await _accountManager.GetLoggedTransaction(new TransactionId((int)InventoryTransactionType.StockAdjustment, stockAdjustmentId)) }
                where x != null
                select x).ToList());
        }

        public async Task<List<ITransaction>> GetNewStockAdjustmentTransactions(Guid stockAdjustmentId)
        {
            var queryGetStockAdjustment = _stockAdjustmentRepository.WithDetails().FirstOrDefault(f => f.Id == stockAdjustmentId);

            if (queryGetStockAdjustment == null)
            {
                return new List<ITransaction>();
            }

            foreach (var line in queryGetStockAdjustment?.Lines ?? Enumerable.Empty<StockAdjustmentLine>())
            {
                //line.Product =await _productRepository.FirstOrDefaultAsync(f => f.Id == line.ProductId);
            }

            var transactions = new List<ITransaction> { await GetStockAdjustment(queryGetStockAdjustment) }.ToList();

            await _accountManager.LoadTransactionAccountIds(transactions);

            return transactions;
        }

        async Task<ITransaction> GetStockAdjustment(StockAdjustment sa)
        {
            if (sa == null || sa.IsCancelled)
            {
                return null;
            }

            var entries = new List<Entry>();


            foreach (var l in sa.Lines)
            {
                entries.Add(new Entry(new AccountInvOnHand(l.ProductId, l.LocationId, l.Sublocation), l.Difference));
                entries.Add(new Entry(new AccountInvAdjustment(l.ProductId), -l.Difference));
                entries.Add(new Entry(new AccountInvTotalOnHand(l.ProductId), l.Difference));
                entries.Add(new Entry(new AccountInvTotalOwned(l.ProductId), l.Difference));
            }

            var ss = new Transaction(new TransactionId((int)InventoryTransactionType.StockAdjustment, sa.Id), sa.CreationTime, entries, true);

            return await Task.FromResult(ss);
        }
    }
}