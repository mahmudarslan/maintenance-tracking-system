using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountInvNonstockedUsed : Account
    {
        public AccountInvNonstockedUsed(Guid id) : base((int)OrderAccountType.InvNonstockedUsed)
        {
            SetProductId(id);
        }
    }
}