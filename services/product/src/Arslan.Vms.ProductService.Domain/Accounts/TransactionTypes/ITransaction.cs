using Arslan.Vms.ProductService.Accounts.AccountTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Services;

namespace Arslan.Vms.ProductService.Accounts.TransactionTypes
{
    public interface ITransaction : IDomainService
    {
        public List<Entry> AllEntries { get; set; }
        public DateTime Date { get; set; }
        public TransactionId Id { get; set; }

        public TransactionOrdering Ordering { get; }

    }
}