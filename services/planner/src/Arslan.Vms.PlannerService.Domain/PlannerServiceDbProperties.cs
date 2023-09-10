namespace Arslan.Vms.PlannerService;

public static class PlannerServiceDbProperties
{
    public static string DbTablePrefix { get; set; } = "PlannerService";

    public static string DbSchema { get; set; } = null;

    public const string ConnectionStringName = "PlannerService";
}
