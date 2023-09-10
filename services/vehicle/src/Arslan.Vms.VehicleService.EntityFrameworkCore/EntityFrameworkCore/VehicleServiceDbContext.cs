using Arslan.Vms.VehicleService.Files;
using Arslan.Vms.VehicleService.Users.UserVehicles;
using Arslan.Vms.VehicleService.Vehicles;
using Arslan.Vms.VehicleService.Vehicles.VehicleTypes;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.VehicleService.EntityFrameworkCore;

[ConnectionStringName(VehicleServiceDbProperties.ConnectionStringName)]
public class VehicleServiceDbContext : AbpDbContext<VehicleServiceDbContext>, IVehicleServiceDbContext
{
    public VehicleServiceDbContext(DbContextOptions<VehicleServiceDbContext> options)
        : base(options)
    {

    }

    #region Vehicle
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleType> VehicleTypes { get; set; }
    #endregion

    public DbSet<UserVehicle> UserVehicle { get; set; }


    #region Attachment
    public DbSet<FileAttachment> FileAttachment { get; set; }
    #endregion   

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureVehicleService();
    }
}
