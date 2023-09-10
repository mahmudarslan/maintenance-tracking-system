using Arslan.Vms.ProductService.Accounts.TransactionTypes;
using Arslan.Vms.ProductService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.ProductService.Accounts
{
    public class BalanceRepository : EfCoreRepository<IProductServiceDbContext, Balances, Guid>, IBalanceRepository
    {
        IDbContextProvider<IProductServiceDbContext> _dbContextProvider;

        public BalanceRepository(IDbContextProvider<IProductServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public async Task<List<Balances>> GetAllBalancesAfterAsync(TransactionOrdering ordering)
        {
            var query = new StringBuilder($"select * from {ProductServiceDbProperties.DbTablePrefix}ACC_Balances");
            query.Append($" where");
            query.Append($" ('{ordering.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}' > LastTransactionDateTime   OR ('{ordering.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}' = LastTransactionDateTime");
            query.Append($" AND ('{ordering.Id.Type}' > LastTransactionType OR ('{ordering.Id.Type}' = LastTransactionType");
            query.Append($" AND ('{ordering.Id.EntityId}' > LastTransactionEntityId OR ('{ordering.Id.EntityId}' = LastTransactionEntityId ");
            query.Append(ordering.Id.ChildId != null ? $"AND ('{ordering.Id.ChildId}' > LastTransactionChildId )" : "AND (null > LastTransactionChildId)");
            query.Append("))))))");

            var data = _dbContextProvider.GetDbContext().Balances.FromSqlRaw(query.ToString());

            return await data.ToListAsync();
        }

    }
}