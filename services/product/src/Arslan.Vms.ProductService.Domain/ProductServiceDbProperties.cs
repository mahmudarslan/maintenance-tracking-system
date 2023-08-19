namespace Arslan.Vms.ProductService;

public static class ProductServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = "ProductService";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "ProductService";
}
