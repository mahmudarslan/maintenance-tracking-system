﻿using Arslan.Vms.OrderService.Accounts.AccountTypes;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.OrderService.Accounts.Transactions
{
    public class LoggedTransactionGroup
    {
        public LoggedTransactionGroup()
        {
            Entries = new List<Entry>();
        }

        public Guid? TransactionChildId { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public decimal Amount { get; set; }
        public Account Account { get; set; }
        public List<Entry> Entries { get; set; }
    }
}