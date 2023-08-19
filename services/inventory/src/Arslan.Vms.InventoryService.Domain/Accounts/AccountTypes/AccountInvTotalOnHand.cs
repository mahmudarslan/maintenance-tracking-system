using System;

namespace Arslan.Vms.InventoryService.Accounts.AccountTypes
{
    public class AccountInvTotalOnHand : Account
    {
        public AccountInvTotalOnHand(Guid tag) : base((int)InventoryAccountType.InvTotalOnHand)
        {
            SetProductId(tag);
        }

        public Account GetAggregateAccount()
        {
            if (AccountType != (int)InventoryAccountType.InvOnHand)
            {
                return null;
            }
            return new AccountInvTotalOnHand(ProductId.Value);
        }
    }
}