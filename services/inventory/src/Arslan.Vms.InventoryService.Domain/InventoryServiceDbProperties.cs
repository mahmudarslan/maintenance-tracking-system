namespace Arslan.Vms.InventoryService;

public static class InventoryServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = "InventoryService";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "InventoryService";
}
