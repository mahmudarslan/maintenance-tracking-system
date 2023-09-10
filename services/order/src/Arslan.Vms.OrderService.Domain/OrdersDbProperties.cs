namespace Arslan.Vms.OrderService
{
    public static class OrdersDbProperties
    {
        public static string DbTablePrefix { get; set; } = "Order";

        public static string DbSchema { get; set; } = null;

        public const string ConnectionStringName = "Order";
    }
}