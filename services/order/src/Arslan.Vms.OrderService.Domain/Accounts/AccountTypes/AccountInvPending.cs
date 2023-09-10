using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountInvPending : Account
    {
        public AccountInvPending(Guid tag) : base((int)OrderAccountType.InvPending)
        {
            SetProductId(tag);
        }
    }
}