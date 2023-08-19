using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountInvServicesReceived : Account
    {
        public AccountInvServicesReceived(Guid productId) : base((int)OrderAccountType.InvServicesReceived)
        {
            SetProductId(productId);
        }
    }
}