using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Arslan.Vms.InventoryService.Accounts.TransactionTypes
{
    public enum InventoryTransactionType
    {
        //[Description("General Journal")]
        //GeneralJournal = 100,
        //CostInitialization = 101,

        [Description("Stock Adjustment")]
        StockAdjustment = 201,
        //[Description("Manual Cost Adjustment")]
        //StandardCostAdjustment = 203,
        //[Description("Batch Overpayment")]
        //BatchOverpayment = 204,
        //[Description("Batch Vendor Overpayment")]
        //BatchVendorOverpayment = 205,
    }
}
