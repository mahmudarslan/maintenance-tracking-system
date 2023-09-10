using System;

namespace Arslan.Vms.OrderService.Accounts.AccountTypes
{
    public class AccountMnyInventoryValue : Account
    {
        public AccountMnyInventoryValue(Guid productId) : base((int)OrderAccountType.MnyInventoryValue)
        {
            SetProductId(productId);
        }
    }
}