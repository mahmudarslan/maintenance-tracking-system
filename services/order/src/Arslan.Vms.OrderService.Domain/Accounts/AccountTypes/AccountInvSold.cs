using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountInvSold : Account
    {
        public Guid CustomerId
        {
            get
            {
                return (Guid)Attr1;
            }
        }

        public AccountInvSold(Guid id, Guid customerId) : base((int)OrderAccountType.InvSold)
        {
            SetProductId(id);
            Attr1 = customerId;
        }
    }
}