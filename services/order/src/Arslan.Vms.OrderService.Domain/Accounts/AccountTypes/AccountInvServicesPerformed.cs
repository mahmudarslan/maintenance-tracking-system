using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountInvServicesPerformed : Account
    {
        public AccountInvServicesPerformed(Guid id) : base((int)OrderAccountType.InvServicesPerformed)
        {
            SetProductId(id);
        }
    }
}