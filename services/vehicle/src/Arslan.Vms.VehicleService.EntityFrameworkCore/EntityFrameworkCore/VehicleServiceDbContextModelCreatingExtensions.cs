using Arslan.Vms.VehicleService.Files;
using Arslan.Vms.VehicleService.Users.UserVehicles;
using Arslan.Vms.VehicleService.Vehicles;
using Arslan.Vms.VehicleService.Vehicles.VehicleTypes;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Arslan.Vms.VehicleService.EntityFrameworkCore;

public static class VehicleServiceDbContextModelCreatingExtensions
{
    public static void ConfigureVehicleService(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));



        #region Vehicle
        builder.Entity<Vehicle>(b =>
        {
            b.ToTable(VehicleServiceDbProperties.DbTablePrefix + "Vehicles", VehicleServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Plate).HasMaxLength(VehicleConsts.PlateMaxLength).IsRequired();
            b.Property(x => x.Chassis).HasMaxLength(VehicleConsts.ChassisMaxLength);
            b.Property(x => x.Color).HasMaxLength(VehicleConsts.ColorMaxLength);
            b.Property(x => x.Motor).HasMaxLength(VehicleConsts.MotorMaxLength);
        });
        builder.Entity<VehicleType>(b =>
        {
            b.ToTable(VehicleServiceDbProperties.DbTablePrefix + "VehicleTypes", VehicleServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Name).HasMaxLength(VehicleTypeConsts.NameMaxLength).IsRequired();
        });
        builder.Entity<UserVehicle>(b =>
        {
            b.ToTable(VehicleServiceDbProperties.DbTablePrefix + "UserVehicles", VehicleServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.HasKey(ur => new { ur.UserId, ur.VehicleId });
            b.HasIndex(ur => new { ur.UserId, ur.VehicleId });
        });
        #endregion



        #region Attachment
        builder.Entity<FileAttachment>(b =>
        {
            b.ToTable(VehicleServiceDbProperties.DbTablePrefix + "FileAttachments", VehicleServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        #endregion


    }
}
