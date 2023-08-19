namespace Arslan.Vms.OrderService;

public static class OrderServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = "OrderService";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "OrderService";
}
