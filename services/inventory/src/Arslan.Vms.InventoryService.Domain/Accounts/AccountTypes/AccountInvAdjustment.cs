using System;

namespace Arslan.Vms.InventoryService.Accounts.AccountTypes
{
    public class AccountInvAdjustment : Account
    {
        public AccountInvAdjustment(Guid tag) : base((int)InventoryAccountType.InvAdjustment)
        {
            SetProductId(tag);
        }
    }
}