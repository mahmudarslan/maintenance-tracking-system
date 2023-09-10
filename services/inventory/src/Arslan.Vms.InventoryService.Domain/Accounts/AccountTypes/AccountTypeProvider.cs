using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Arslan.Vms.InventoryService.Accounts.AccountTypes
{
    public class AccountTypeProvider : IAccountTypeProvider, ITransientDependency
    {
        public AccountTypeProvider()
        {
        }

        public List<Entry> GetAggregates(List<Entry> primitiveEntries)
        {
            return primitiveEntries.SelectMany(new Func<Entry, List<Entry>>(GetAggregates)).ToList();
        }

        private List<Entry> GetAggregates(Entry e)
        {
            return (from f in GetAggregateTypes()
                    select f(e.Account) into aggregateAccount
                    where aggregateAccount != null
                    select new Entry(aggregateAccount, e.Amount)).ToList();
        }

        private static IEnumerable<Func<Account, Account>> GetAggregateTypes()
        {
            yield return null;
            //yield return new Func<Account, Account>(AccountInvTotalOnHand.GetAggregateAccount());
            //yield return new Func<Account, Account>(AccountInvTotalOwned.GetAggregateAccount());
            //yield return new Func<Account, Account>(AccountInvTotalBySerial.GetAggregateAccount);
        }

        public bool IsAggregate(InventoryAccountType accountType)
        {
            return accountType.IsAggregate();
        }

        public bool IsFinancial(InventoryAccountType accountType)
        {
            return accountType.IsFinancial();
        }

        public bool IsInventory(InventoryAccountType accountType)
        {
            return accountType.IsInventory();
        }
    }
}