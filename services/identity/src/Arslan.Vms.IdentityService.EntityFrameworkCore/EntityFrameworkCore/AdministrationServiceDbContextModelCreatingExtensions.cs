using Arslan.Vms.IdentityService.Addresses;
using Arslan.Vms.IdentityService.Addresses.AddressTypes;
using Arslan.Vms.IdentityService.Addresses.Version;
using Arslan.Vms.IdentityService.Companies;
using Arslan.Vms.IdentityService.Files;
using Arslan.Vms.IdentityService.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Arslan.Vms.IdentityService.EntityFrameworkCore;

public static class IdentityServiceDbContextModelCreatingExtensions
{
    public static void ConfigureIdentityService(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(IdentityServiceDbProperties.DbTablePrefix + "Questions", IdentityServiceDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */

        #region Address
        builder.Entity<Address>(b =>
        {
            b.ToTable(IdentityServiceDbProperties.DbTablePrefix + "Address", IdentityServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<AddressVersion>(b =>
        {
            b.ToTable(IdentityServiceDbProperties.DbTablePrefix + "AddressVersions", IdentityServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        #endregion

        #region Address Type
        builder.Entity<AddressType>(b =>
        {
            b.ToTable(IdentityServiceDbProperties.DbTablePrefix + "AddressTypes", IdentityServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Name).HasMaxLength(AddressTypeConsts.NameMaxLength);
        });
        #endregion

        #region Company
        builder.Entity<Company>(b =>
        {
            b.ToTable(IdentityServiceDbProperties.DbTablePrefix + "Companies", IdentityServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Name).IsRequired();
        });
        //builder.Entity<CompanyPreference>(b =>
        //{
        //    b.ToTable(VehicleServiceDbProperties.DbTablePrefix + "CompanyPreferences", VehicleServiceDbProperties.DbSchema);
        //    b.ConfigureByConvention();
        //});
        builder.Entity<CompanyAddress>(b =>
        {
            b.ToTable(IdentityServiceDbProperties.DbTablePrefix + "CompanyAddress", IdentityServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            //b.HasOne<Address>().WithMany().HasForeignKey(h => h.AddressId);
            b.HasKey(h => new { h.CompanyId, h.AddressId });
        });
        builder.Entity<CompanyAttachment>(b =>
        {
            b.ToTable(IdentityServiceDbProperties.DbTablePrefix + "CompanyAttachments", IdentityServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<FileAttachment>().WithMany().HasForeignKey(h => h.FileAttachmentId);
            b.HasKey(h => new { h.CompanyId, h.FileAttachmentId });
        });
        #endregion

        #region Customer
        //builder.Entity<CustomerPayment>(b =>
        //{
        //    b.ToTable(VehicleServiceDbProperties.DbTablePrefix + "CustomerPayments", VehicleServiceDbProperties.DbSchema);
        //    b.ConfigureByConvention();
        //    b.ConfigureFullAuditedAggregateRoot();
        //});
        #endregion

        #region User
        builder.Entity<User>(b =>
        {
            b.ToTable(IdentityServiceDbProperties.DbTablePrefix + "Users");
            b.ConfigureByConvention();
            //b.ConfigureAbpUser();
            b.Property(x => x.Discount).HasColumnType("decimal(5,2)");
            b.Property(x => x.VendorPermitNumber).HasMaxLength(UserConsts.VendorPermitNumberMaxLength);
            b.Property(x => x.DefaultCarrier).HasMaxLength(UserConsts.DefaultCarrierMaxLength);
            b.Property(x => x.DefaultPaymentMethod).HasMaxLength(UserConsts.DefaultPaymentMethodMaxLength);
        });
        builder.Entity<Role>(b =>
        {
            b.ToTable(IdentityServiceDbProperties.DbTablePrefix + "Roles");
            b.ConfigureByConvention();
        });
        builder.Entity<UserRole>(b =>
        {
            b.ToTable(IdentityServiceDbProperties.DbTablePrefix + "UserRoles");
            b.ConfigureByConvention();
            b.HasKey(h => new { h.UserId, h.RoleId });
        });
        builder.Entity<UserAddress>(b =>
        {
            b.ToTable(IdentityServiceDbProperties.DbTablePrefix + "UserAddresses", IdentityServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.HasKey(h => new { h.UserId, h.AddressId });
        });
        //builder.Entity<UserVehicle>(b =>
        //{
        //    b.ToTable(VehicleServiceDbProperties.DbTablePrefix + "UserVehicles", VehicleServiceDbProperties.DbSchema);
        //    b.ConfigureByConvention();
        //    b.HasKey(ur => new { ur.UserId, ur.VehicleId });
        //    b.HasIndex(ur => new { ur.UserId, ur.VehicleId });
        //});
        #endregion
    }
}
