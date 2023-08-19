using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountInvPurchased : Account
    {
        public Guid VendorId
        {
            get
            {
                return (Guid)Attr1;
            }
        }

        public AccountInvPurchased(Guid productId, Guid vendorId) : base((int)OrderAccountType.InvPurchased)
        {
            SetProductId(productId);
            Attr1 = vendorId;
        }
    }
}