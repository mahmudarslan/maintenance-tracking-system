using Arslan.Vms.ProductService.Accounts.TransactionTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.ProductService.Accounts
{
    public interface IBalanceRepository : IRepository<Balances, Guid>
    {
        Task<List<Balances>> GetAllBalancesAfterAsync(TransactionOrdering ordering);
    }
}