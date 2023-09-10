using Arslan.Vms.InventoryService.Accounts.TransactionTypes;
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.InventoryService.Accounts
{
    public class Checkpoint : BasicAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }
        public virtual int TransactionType { get; set; }
        public virtual Guid TransactionEntityId { get; set; }
        public virtual Guid? TransactionChildId { get; set; }
        public virtual Guid AccountId { get; set; }
        public virtual decimal Amount { get; set; }
        public virtual decimal BalanceAfter { get; set; }
        public virtual DateTime TransactionDateTime { get; set; }

        protected Checkpoint() { }

        public Checkpoint(Guid id, Guid? tenantId, Guid accountId, int transactionType, DateTime transactionDateTime, Guid transactionId, decimal amount, decimal balanceAfter, Guid? transactionChildId = null) : base(id)
        {
            TenantId = tenantId;
            AccountId = accountId;
            TransactionType = transactionType;
            TransactionEntityId = transactionId;
            TransactionChildId = transactionChildId;
            Amount = amount;
            BalanceAfter = balanceAfter;
            TransactionDateTime = transactionDateTime;
        }

        public TransactionOrdering TransactionOrdering
        {
            get
            {
                return new TransactionOrdering(TransactionDateTime, TransactionType, TransactionEntityId, TransactionChildId);
            }
        }
    }
}
