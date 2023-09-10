using System.Collections.Generic;

namespace Arslan.Vms.InventoryService.Accounts.AccountTypes
{
    public interface IAccountTypeProvider
    {
        List<Entry> GetAggregates(List<Entry> primitiveEntries);

        bool IsAggregate(InventoryAccountType accountType);

        bool IsFinancial(InventoryAccountType accountType);

        bool IsInventory(InventoryAccountType accountType);
    }
}
