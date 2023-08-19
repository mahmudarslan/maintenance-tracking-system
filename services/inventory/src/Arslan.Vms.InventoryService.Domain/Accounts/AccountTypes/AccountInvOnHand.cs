using System;

namespace Arslan.Vms.InventoryService.Accounts.AccountTypes
{
    public class AccountInvOnHand : Account
    {
        public Guid LocationId
        {
            get
            {
                return (Guid)Attr1;
            }
        }

        public AccountInvOnHand(Guid productId, Guid locationId, string sublocation) : base((int)InventoryAccountType.InvOnHand)
        {
            Attr1 = locationId;
            Sublocation = sublocation;
            SetProductId(productId);
        }
    }
}