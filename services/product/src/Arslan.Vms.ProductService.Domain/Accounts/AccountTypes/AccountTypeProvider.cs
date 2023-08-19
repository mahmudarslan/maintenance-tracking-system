using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Arslan.Vms.ProductService.Accounts.AccountTypes
{
    public class AccountTypeProvider : IAccountTypeProvider, ITransientDependency
    {
        public AccountTypeProvider()
        {
        }

        public List<Entry> GetAggregates(List<Entry> primitiveEntries)
        {
            //return primitiveEntries.SelectMany(new Func<Entry, List<Entry>>(GetAggregates)).ToList(); 
            //var aggregateTypes = primitiveEntries.Where(w => w.Account.GetAggregateAccount() != null).ToList();

            //foreach (var item in primitiveEntries.Where(w => w.Account.GetAggregateAccount() == null))
            //{
            //    foreach (var item1 in aggregateTypes)
            //    {
            //        item1.Amount = item.Amount;
            //    }
            //}

            return primitiveEntries;
        }

        //private List<Entry> GetAggregates(Entry e)
        //{
        //    return (from f in GetAggregateTypes() select f(e.Account) into aggregateAccount
        //            where aggregateAccount != null 
        //            select new Entry(aggregateAccount, e.Amount)).ToList();
        //}

        //private static IEnumerable<Func<Account, Account>> GetAggregateTypes()
        //{
        //    yield return new Func<Account, Account>(AccountInvTotalOnHand.GetAggregateAccount);
        //    yield return new Func<Account, Account>(AccountInvTotalOwned.GetAggregateAccount);
        //}

        public bool IsAggregate(AccountType accountType)
        {
            return accountType.IsAggregate();
        }

        public bool IsFinancial(AccountType accountType)
        {
            return accountType.IsFinancial();
        }

        public bool IsInventory(AccountType accountType)
        {
            return accountType.IsInventory();
        }
    }
}