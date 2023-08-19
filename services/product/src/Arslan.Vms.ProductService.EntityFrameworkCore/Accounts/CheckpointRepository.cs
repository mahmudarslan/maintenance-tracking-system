using Arslan.Vms.ProductService.Accounts.AccountTypes;
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
    public class CheckpointRepository : EfCoreRepository<IProductServiceDbContext, Checkpoint, Guid>, ICheckpointRepository
    {
        IDbContextProvider<IProductServiceDbContext> _dbContextProvider;

        public CheckpointRepository(IDbContextProvider<IProductServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public async Task<List<Checkpoint>> GetAllCheckpointsAfterAsync(Entry entry, TransactionOrdering ordering)
        {
            var query = new StringBuilder($"select * from {ProductServiceDbProperties.DbTablePrefix}ACC_Checkpoints");
            query.Append($" where  (AccountId='{entry.AccountId}')  ");
            query.Append($" AND (TransactionDateTime > '{ordering.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}' OR (TransactionDateTime = '{ordering.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}'");
            query.Append($" AND (TransactionType > '{ordering.Id.Type}' OR (TransactionType = '{ordering.Id.Type}'");
            query.Append($" AND (TransactionEntityId > '{ordering.Id.EntityId}' OR (TransactionEntityId = '{ordering.Id.EntityId}'");
            query.Append(ordering.Id.ChildId != null ? $" AND (TransactionChildId >'{ordering.Id.ChildId}')" : " AND (TransactionChildId > null)");
            query.Append("))))))");

            var data = _dbContextProvider.GetDbContext().Checkpoint.FromSqlRaw(query.ToString());

            return await data.ToListAsync();
        }

        public async Task<decimal> FindTheNewLastBalanceAsync(Entry entry, TransactionOrdering ordering)
        {
            var query = new StringBuilder($"select * from {ProductServiceDbProperties.DbTablePrefix}ACC_Checkpoints");
            query.Append($" where  (AccountId='{entry.AccountId}')");
            query.Append($" AND (TransactionDateTime < '{ordering.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}' OR (TransactionDateTime = '{ordering.DateTime.ToString("yyyy-MM-dd HH:mm:ss.fffffff")}'");
            query.Append($" AND (TransactionType < '{ordering.Id.Type}' OR (TransactionType = '{ordering.Id.Type}'");
            query.Append($" AND (TransactionEntityId < '{ordering.Id.EntityId}' OR (TransactionEntityId = '{ordering.Id.EntityId}'");
            query.Append(ordering.Id.ChildId != null ? $" AND (TransactionChildId <'{ordering.Id.ChildId}')" : " AND (TransactionChildId < null)");
            query.Append("))))))");

            var data = await _dbContextProvider.GetDbContext().Checkpoint.FromSqlRaw(query.ToString()).FirstOrDefaultAsync();

            return data != null ? data.BalanceAfter : 0;
        }
    }
}