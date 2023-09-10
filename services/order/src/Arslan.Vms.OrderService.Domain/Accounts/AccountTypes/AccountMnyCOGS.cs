using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountMnyCOGS : Account
    {
        public AccountMnyCOGS(Guid productId) : base((int)OrderAccountType.MnyCOGS)
        {
            SetProductId(productId);
        }
    }
}