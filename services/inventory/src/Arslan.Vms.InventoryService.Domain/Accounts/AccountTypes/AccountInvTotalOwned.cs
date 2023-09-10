using System;

namespace Arslan.Vms.InventoryService.Accounts.AccountTypes
{
    public class AccountInvTotalOwned : Account
    {
        public AccountInvTotalOwned(Guid productId) : base((int)InventoryAccountType.InvTotalOwned)
        {
            SetProductId(productId);
        }

        public Account GetAggregateAccount()
        {
            switch (AccountType)
            {
                case (int)InventoryAccountType.InvOnHand:
                    //case (int)InventoryAccountType.InvPicked:
                    //case (int)InventoryAccountType.InvInTransit:
                    //case (int)InventoryAccountType.InvWorkOrderPicked:
                    //case (int)InventoryAccountType.InvWorkOrderFinished:
                    {
                        return new AccountInvTotalOwned(ProductId.Value);
                    }
                //case (int)InventoryAccountType.InvOnOrder:
                //case (int)InventoryAccountType.InvOnWorkOrder:
                //{
                //    return null;
                //}
                default:
                    {
                        return null;
                    }
            }
        }
    }
}