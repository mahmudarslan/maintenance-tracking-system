using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountMnyServiceCosts : Account
    {
        public AccountMnyServiceCosts(Guid productId) : base((int)OrderAccountType.MnyServiceCosts)
        {
            SetProductId(productId);
        }
    }
}