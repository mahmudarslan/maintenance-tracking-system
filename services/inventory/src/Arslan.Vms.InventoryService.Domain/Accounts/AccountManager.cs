using Arslan.Vms.InventoryService.Accounts.AccountTypes;
using Arslan.Vms.InventoryService.Accounts.TransactionTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.InventoryService.Accounts
{
    public class AccountManager : DomainService
    {
        private readonly IRepository<Account, Guid> _accountRepository;
        private readonly IBalanceRepository _balanceRepository;
        private readonly ICheckpointRepository _checkpointRepository;
        private readonly ICurrentTenant _currentTenant;

        public AccountManager(
               IRepository<Account, Guid> accountRepository,
               IBalanceRepository balanceRepository,
               ICheckpointRepository checkpointRepository,
               ICurrentTenant currentTenant)
        {
            _accountRepository = accountRepository;
            _currentTenant = currentTenant;
            _checkpointRepository = checkpointRepository;
            _balanceRepository = balanceRepository;
        }

        public bool SuspendAccountingCalculations
        {
            get;
            set;
        }

        public async Task<ITransaction> GetLoggedTransaction(TransactionId id)
        {
            ITransaction transaction;
            var entries = new List<Entry>();
            var minValue = DateTime.MinValue;

            var checkpointRepository = await _checkpointRepository.GetQueryableAsync();
            var accountRepository = await _accountRepository.GetQueryableAsync();

            var datas = (from c in checkpointRepository
                         from a in accountRepository.Where(w => w.Id == c.AccountId).DefaultIfEmpty()
                         where c.TransactionType == id.Type && c.TransactionEntityId == id.EntityId && c.TransactionChildId == id.ChildId
                         select new LoggedTransactionGroup
                         {
                             TransactionChildId = c.TransactionChildId,
                             TransactionDateTime = c.TransactionDateTime,
                             Amount = c.Amount,
                             Account = a,
                         }).ToList();

            foreach (var item in datas)
            {
                minValue = item.TransactionDateTime;
                entries.Add(new Entry(item.Account, item.Amount));
            }

            if (entries.Count == 0)
            {
                transaction = null;
            }
            else
            {
                transaction = new Transaction(id, minValue, entries, false);
            }

            return await Task.FromResult(transaction);
        }


        public async Task<IEnumerable<ITransaction>> GetLoggedTransactionGroup(int transactionType, Guid transactionEntityId)
        {
            var checkpointRepository = await _checkpointRepository.GetQueryableAsync();
            var accountRepository = await _accountRepository.GetQueryableAsync();

            var datas = (from c in checkpointRepository
                         from a in accountRepository.Where(w => w.Id == c.AccountId)
                         where c.TransactionType == transactionType && c.TransactionEntityId == transactionEntityId
                         select new LoggedTransactionGroup
                         {
                             TransactionChildId = c.TransactionChildId,
                             TransactionDateTime = c.TransactionDateTime,
                             Amount = c.Amount,
                             Account = a,
                         }).ToList();

            var cc = datas.GroupBy(g => new { g.TransactionChildId, g.TransactionDateTime })
                .Select(s => new LoggedTransactionGroup
                {
                    TransactionChildId = s.Key.TransactionChildId,
                    TransactionDateTime = s.Key.TransactionDateTime,
                    Entries = s.ToList().Select(se => new Entry(se.Account, se.Amount)).ToList()
                }).ToList();

            var transactions = (from x in cc select new Transaction(new TransactionId(transactionType, transactionEntityId, x.TransactionChildId), x.TransactionDateTime, x.Entries, false)).ToList();

            return await Task.FromResult(transactions);
        }

        async Task<TransactionOrdering> GetLastTransactionOrderingFromCheckpoint(Account account)
        {
            TransactionOrdering transactionOrdering = null;
            var checkpointRepository = await _checkpointRepository.GetQueryableAsync();

            var checkpoint = checkpointRepository.Where(w => w.AccountId == account.Id)
               .OrderByDescending(o => o.TransactionDateTime)
               .ThenByDescending(o => o.TransactionType)
               .ThenByDescending(o => o.TransactionEntityId)
               .ThenByDescending(o => o.TransactionChildId).FirstOrDefault();

            if (checkpoint != null)
            {
                transactionOrdering = new TransactionOrdering(
                    checkpoint.TransactionDateTime,
                    checkpoint.TransactionType,
                    checkpoint.TransactionEntityId,
                    checkpoint.TransactionChildId);
            }

            return await Task.FromResult(transactionOrdering);
        }
        async Task<TransactionOrdering> GetLastTransactionOrderingFromBalance(Guid accountId)
        {
            TransactionOrdering queryGetLastTransactionOrdering = null;
            var balanceRepository = await _balanceRepository.GetQueryableAsync();

            var balance = balanceRepository.Where(w => w.AccountId == accountId)
               .OrderByDescending(o => o.LastTransactionDateTime)
               .ThenByDescending(o => o.LastTransactionType)
               .ThenByDescending(o => o.LastTransactionEntityId)
               .ThenByDescending(o => o.LastTransactionChildId).FirstOrDefault();

            if (balance != null)
            {
                queryGetLastTransactionOrdering = new TransactionOrdering(balance.LastTransactionDateTime,
                                                                          balance.LastTransactionType,
                                                                          balance.LastTransactionEntityId,
                                                                          balance.LastTransactionChildId);
            }

            return await Task.FromResult(queryGetLastTransactionOrdering);
        }

        public async Task<Account> AccountLoadId(Account account)
        {
            if (!account.IdInitialized)
            {
                var queryGetAccount = await _accountRepository.FirstOrDefaultAsync(f =>
                 f.AccountType == account.AccountType &&
                 f.Attr1 == account.Attr1 &&
                 f.Attr2 == account.Attr2 &&
                 f.ProductId == account.ProductId &&
                 f.Sublocation == account.Sublocation &&
                 f.CurrencyId == account.CurrencyId);

                if (queryGetAccount == null)
                {
                    await _accountRepository.InsertAsync(account, true);
                }
                else
                {
                    account = queryGetAccount;
                }
            }

            return await Task.FromResult(account);
        }

        public async Task LoadTransactionAccountIds(List<ITransaction> txs)
        {
            var entries = txs.SelectMany(t => t.AllEntries).GroupBy(e => e.Account).ToArray();

            foreach (var entry in entries)
            {
                foreach (var item in entry)
                {
                    if (item.Account.IdInitialized == false)
                    {
                        item.Account = await AccountLoadId(item.Account);
                    }
                }
            }
        }

        public async Task UpdateTransactions(List<ITransaction> oldTxs,
            List<ITransaction> newTxs,
            TransactionOrdering updateFrom = null,
            bool doCostInitialization = true,
            bool allowNegativeInventory = false)
        {
            newTxs = (from x in newTxs orderby x.Ordering select x).ToList();

            await LoadTransactionAccountIds(newTxs);

            var productTags = await UpdateBaseTransactions(oldTxs, newTxs, updateFrom, allowNegativeInventory);

            if (productTags == null)
            {
                return;
            }
        }


        public async Task<Dictionary<Guid, TransactionOrdering>> UpdateBaseTransactions(
            List<ITransaction> oldTxs,
            List<ITransaction> newTxs,
            TransactionOrdering updateFrom,
            bool allowNegativeInventory = false)
        {
            var hashSet = oldTxs.SelectMany(t => from e in t.AllEntries select Tuple.Create(t.Ordering, e)).ToList();//.ToHashSet(null);

            var tuples = newTxs.SelectMany(t => from e in t.AllEntries select Tuple.Create(t.Ordering, e)).ToList();//.ToHashSet(null);

            var list = (from x in hashSet where !tuples.Contains(x) select x).ToList();

            var list1 = (from x in tuples where !hashSet.Contains(x) select x).ToList();

            foreach (Tuple<TransactionOrdering, Entry> tuple in list)
            {
                await RemoveEntryFromBalances(tuple.Item1, tuple.Item2);
            }


            foreach (Tuple<TransactionOrdering, Entry> tuple1 in list1)
            {
                await AddEntryToBalances(tuple1.Item1, tuple1.Item2);
            }

            //return AccountingFunctions.UpdateFirstCOGS(hashSet, tuples);
            return null;
        }

        async Task AddEntryToBalances(TransactionOrdering ordering, Entry entry)
        {
            var _balance = await _balanceRepository.FirstOrDefaultAsync(w => w.AccountId == entry.AccountId);

            //First checkpoint for this account
            if (_balance == null)
            {
                var balance = new Balances(
                            _currentTenant.Id,
                            entry.AccountId,
                            ordering.Id.Type,
                            ordering.DateTime,
                            ordering.Id.EntityId,
                            entry.Amount,
                            ordering.Id.ChildId);
                await _balanceRepository.InsertAsync(balance, true);

                var checkpoint = new Checkpoint(GuidGenerator.Create(),
                            _currentTenant.Id,
                            entry.AccountId,
                            ordering.Id.Type,
                            ordering.DateTime,
                            ordering.Id.EntityId,
                            entry.Amount,
                            entry.Amount,
                            ordering.Id.ChildId);
                await _checkpointRepository.InsertAsync(checkpoint, true);
            }
            else
            {
                //If this is going to be the newest
                var aa = await _balanceRepository.GetAllBalancesAfterAsync(ordering);

                if (aa.Count > 0)
                {
                    //Newest-- update balance(and last) and update checkpoint
                    _balance.Balance += entry.Amount;
                    _balance.LastTransactionDateTime = ordering.DateTime;
                    _balance.LastTransactionType = ordering.Id.Type;
                    _balance.LastTransactionEntityId = ordering.Id.EntityId;
                    _balance.LastTransactionChildId = ordering.Id.ChildId;
                    await _balanceRepository.UpdateAsync(_balance, true);

                    var checkpoint = new Checkpoint(GuidGenerator.Create(),
                                _currentTenant.Id,
                                entry.AccountId,
                                ordering.Id.Type,
                                ordering.DateTime,
                                ordering.Id.EntityId,
                                entry.Amount,
                                _balance.Balance,
                                ordering.Id.ChildId);

                    await _checkpointRepository.InsertAsync(checkpoint, true);
                }
                //This is not going to be the newest, update existing checkpoints too
                else
                {
                    _balance.Balance += entry.Amount;
                    await _balanceRepository.UpdateAsync(_balance, true);

                    //Find the new @LastBalance
                    var lastBalance = await _checkpointRepository.FindTheNewLastBalanceAsync(entry, ordering);

                    //Update all following checkpoints
                    var checkpoints = await _checkpointRepository.GetAllCheckpointsAfterAsync(entry, ordering);

                    foreach (var checkpoint in checkpoints)
                    {
                        checkpoint.BalanceAfter += entry.Amount;
                        await _checkpointRepository.UpdateAsync(checkpoint, true);
                    }

                    //Insert this checkpoint
                    var newCheckpoint = new Checkpoint(GuidGenerator.Create(),
                                 _currentTenant.Id,
                                 entry.AccountId,
                                 ordering.Id.Type,
                                 ordering.DateTime,
                                 ordering.Id.EntityId,
                                 entry.Amount,
                                 entry.Amount + lastBalance,
                                 ordering.Id.ChildId);

                    await _checkpointRepository.InsertAsync(newCheckpoint, true);
                }
            }
        }

        async Task RemoveEntryFromBalances(TransactionOrdering ordering, Entry e)
        {
            //Remove Checkpoint
            var checkpointRepository = await _checkpointRepository.GetQueryableAsync();
            var checkpoints = checkpointRepository.Where(w => w.TransactionType == ordering.Id.Type &&
                                                                      w.TransactionEntityId == ordering.Id.EntityId &&
                                                                      w.TransactionChildId == ordering.Id.ChildId &&
                                                                      w.AccountId == e.AccountId).ToList();
            foreach (var checkpoint in checkpoints)
            {
                await _checkpointRepository.DeleteAsync(checkpoint, true);
            }

            //Substruct Entry
            var entry = new Entry(e.Account, -e.Amount);

            var lastTransactionOrderingFromBalance = await GetLastTransactionOrderingFromBalance(e.AccountId);
            //current ordering and last transaction ordering is not equal then upated balance and checkpoint
            if (ordering != lastTransactionOrderingFromBalance)//Update
            {
                await UpdateAllCheckpointsAfter(entry, ordering);
                await UpdateCurrentBalance(entry.AccountId, entry.Amount);
            }
            else
            {
                //if chekpoint is null then delete balance
                var lastTransactionOrderingFromCheckpoint = await GetLastTransactionOrderingFromCheckpoint(e.Account);

                if (lastTransactionOrderingFromCheckpoint == null)
                { //Delete Balance
                    await PurgeBalance(e.Account);
                }
                else
                {
                    //Update Blance 
                    var balanceRepository = await _balanceRepository.GetQueryableAsync();
                    var balances = balanceRepository.Where(f => f.AccountId == e.AccountId).ToList();

                    foreach (var balance in balances)
                    {
                        balance.Balance += entry.Amount;
                        balance.LastTransactionDateTime = lastTransactionOrderingFromCheckpoint.DateTime;
                        balance.LastTransactionType = lastTransactionOrderingFromCheckpoint.Id.Type;
                        balance.LastTransactionEntityId = lastTransactionOrderingFromCheckpoint.Id.EntityId;
                        balance.LastTransactionChildId = lastTransactionOrderingFromCheckpoint.Id.ChildId;
                        await _balanceRepository.UpdateAsync(balance, true);
                    }
                }

            }
        }

        async Task UpdateAllCheckpointsAfter(Entry entry, TransactionOrdering ordering)
        {
            var checkpoints = await _checkpointRepository.GetAllCheckpointsAfterAsync(entry, ordering);

            foreach (var checkpoint in checkpoints)
            {
                checkpoint.BalanceAfter += entry.Amount;
                await _checkpointRepository.UpdateAsync(checkpoint, true);
            }
        }

        async Task UpdateCurrentBalance(Guid accountId, decimal amount)
        {
            var balanceRepository = await _balanceRepository.GetQueryableAsync();

            var balances = balanceRepository.Where(w => w.AccountId == accountId).ToList();

            foreach (var balance in balances)
            {
                balance.Balance += amount;
                await _balanceRepository.UpdateAsync(balance, true);
            }
        }

        async Task PurgeBalance(Account account)
        {
            var balanceRepository = await _balanceRepository.GetQueryableAsync();

            var balances = balanceRepository.Where(w => w.AccountId == account.Id).ToList();

            foreach (var balance in balances)
            {
                await _balanceRepository.DeleteAsync(balance, true);
            }
        }
    }
}