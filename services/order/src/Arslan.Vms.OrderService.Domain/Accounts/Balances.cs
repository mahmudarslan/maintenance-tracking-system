using Arslan.Vms.OrderService.Accounts.Transactions;
using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.Accounts
{
    public class Balances : BasicAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }
        public virtual Guid AccountId { get; set; }
        public virtual int LastTransactionType { get; set; }
        public virtual Guid LastTransactionEntityId { get; set; }
        public virtual Guid? LastTransactionChildId { get; set; }
        public virtual DateTime LastTransactionDateTime { get; set; }
        public virtual decimal Balance { get; set; }

        protected Balances() { }

        public Balances(Guid? tenantId, Guid accountId, int lastTransactionType, DateTime lastTransactionDateTime, Guid lastTransactionId, decimal balance, Guid? lastTransactionChildId = null)
        {
            TenantId = tenantId;
            AccountId = accountId;
            LastTransactionType = lastTransactionType;
            LastTransactionEntityId = lastTransactionId;
            LastTransactionChildId = lastTransactionChildId;
            Balance = balance;
            LastTransactionDateTime = lastTransactionDateTime;
        }

        public TransactionOrdering LastTransactionOrdering
        {
            get
            {
                return new TransactionOrdering(LastTransactionDateTime, LastTransactionType, LastTransactionEntityId, LastTransactionChildId);
            }
        }
    }
}
