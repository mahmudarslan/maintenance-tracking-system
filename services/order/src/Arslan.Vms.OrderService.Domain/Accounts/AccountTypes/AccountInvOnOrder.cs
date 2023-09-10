using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountInvOnOrder : Account
    {
        public AccountInvOnOrder(Guid productId) : base((int)OrderAccountType.InvOnOrder)
        {
            SetProductId(productId);
        }
    }
}