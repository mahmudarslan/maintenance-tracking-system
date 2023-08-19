namespace Arslan.Vms.ProductService.Accounts.AccountTypes
{
    public static class AccountTypeExtensions
    {
        public static bool IsAggregate(this AccountType t)
        {
            var value = (int)t;
            return value >= 3000 && value < 4000;
        }

        public static bool IsFinancial(this AccountType t)
        {
            var value = (int)t;
            return value >= 2000 && value < 3000 || value >= 4000;
        }

        public static bool IsInventory(this AccountType t)
        {
            var value = (int)t;
            return value >= 3000 && value < 4000 || value < 2000;
        }

        public static bool IsTrading(this AccountType t)
        {
            var value = (int)t;
            return value >= 5000 && value < 6000;
        }

        public static bool IsSimpleFinancial(this AccountType t)
        {
            return t.IsFinancial() && !t.IsAggregate() && !t.IsTrading();
        }

        public static bool IsSimpleFinancialOrTrading(this AccountType t)
        {
            return t.IsFinancial() && !t.IsAggregate();
        }

        public static bool IsSimpleInventory(this AccountType t)
        {
            return t.IsInventory() && !t.IsAggregate();
        }
    }
}
