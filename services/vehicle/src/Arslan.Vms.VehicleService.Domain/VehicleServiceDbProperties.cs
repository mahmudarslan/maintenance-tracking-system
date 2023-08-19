namespace Arslan.Vms.VehicleService;

public static class VehicleServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = "VehicleService";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "VehicleService";
}
