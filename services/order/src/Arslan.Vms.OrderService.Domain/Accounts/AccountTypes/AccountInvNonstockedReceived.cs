using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountInvNonstockedReceived : Account
    {
        public AccountInvNonstockedReceived(Guid productId) : base((int)OrderAccountType.InvNonstockedReceived)
        {
            SetProductId(productId);
        }
    }
}