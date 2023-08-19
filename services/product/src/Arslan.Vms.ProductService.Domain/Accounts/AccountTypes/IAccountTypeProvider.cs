using System.Collections.Generic;

namespace Arslan.Vms.ProductService.Accounts.AccountTypes
{
    public interface IAccountTypeProvider
    {
        List<Entry> GetAggregates(List<Entry> primitiveEntries);

        bool IsAggregate(AccountType accountType);

        bool IsFinancial(AccountType accountType);

        bool IsInventory(AccountType accountType);
    }
}