using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountMnyNonstockedCosts : Account
    {
        public AccountMnyNonstockedCosts(Guid productId) : base((int)OrderAccountType.MnyNonstockedCosts)
        {
            SetProductId(productId);
        }
    }
}