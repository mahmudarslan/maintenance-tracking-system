using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.VehicleService.EntityFrameworkCore;

[ConnectionStringName(VehicleServiceDbProperties.ConnectionStringName)]
public interface IVehicleServiceDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
