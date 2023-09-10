using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountInvPicked : Account
    {
        public AccountInvPicked(Guid productId) : base((int)OrderAccountType.InvPicked)
        {
            SetProductId(productId);
        }
    }
}