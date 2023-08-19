namespace Arslan.Vms.InventoryService.Accounts.AccountTypes
{
    public static class InventoryAccountTypeExtensions
    {
        public static bool IsAggregate(this InventoryAccountType t)
        {
            int ınt32 = (int)t;
            if (ınt32 < 3000)
            {
                return false;
            }
            return ınt32 < 4000;
        }

        public static bool IsFinancial(this InventoryAccountType t)
        {
            int ınt32 = (int)t;
            if (ınt32 >= 4000)
            {
                return true;
            }
            if (ınt32 < 2000)
            {
                return false;
            }
            return ınt32 < 3000;
        }

        public static bool IsInventory(this InventoryAccountType t)
        {
            int ınt32 = (int)t;
            if (ınt32 < 2000)
            {
                return true;
            }
            if (ınt32 < 3000)
            {
                return false;
            }
            return ınt32 < 4000;
        }

        public static bool IsSimpleFinancial(this InventoryAccountType t)
        {
            if (!t.IsFinancial() || t.IsAggregate())
            {
                return false;
            }
            return !t.IsTrading();
        }

        public static bool IsSimpleFinancialOrTrading(this InventoryAccountType t)
        {
            if (!t.IsFinancial())
            {
                return false;
            }
            return !t.IsAggregate();
        }

        public static bool IsSimpleInventory(this InventoryAccountType t)
        {
            if (!t.IsInventory())
            {
                return false;
            }
            return !t.IsAggregate();
        }

        public static bool IsTrading(this InventoryAccountType t)
        {
            int ınt32 = (int)t;
            if (ınt32 < 5000)
            {
                return false;
            }
            return ınt32 < 6000;
        }
    }
}
