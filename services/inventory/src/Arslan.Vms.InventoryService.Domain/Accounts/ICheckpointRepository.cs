using Arslan.Vms.InventoryService.Accounts.AccountTypes;
using Arslan.Vms.InventoryService.Accounts.TransactionTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.InventoryService.Accounts
{
    public interface ICheckpointRepository : IRepository<Checkpoint, Guid>
    {
        Task<List<Checkpoint>> GetAllCheckpointsAfterAsync(Entry entry, TransactionOrdering ordering);
        Task<decimal> FindTheNewLastBalanceAsync(Entry entry, TransactionOrdering ordering);
    }
}