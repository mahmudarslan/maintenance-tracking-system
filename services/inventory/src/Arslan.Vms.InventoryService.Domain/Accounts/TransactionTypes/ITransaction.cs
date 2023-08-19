using Arslan.Vms.InventoryService.Accounts.AccountTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.Domain.Services;

namespace Arslan.Vms.InventoryService.Accounts.TransactionTypes
{
    public interface ITransaction : IDomainService
    {
        public List<Entry> AllEntries { get; set; }
        public DateTime Date { get; set; }
        public TransactionId Id { get; set; }

        public TransactionOrdering Ordering { get; }
        //{
        //    get
        //    {
        //        return new TransactionOrdering(Id, Date);
        //    }
        //}

    }
}