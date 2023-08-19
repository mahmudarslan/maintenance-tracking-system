using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.AuditLogging;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Arslan.Vms.IdentityService.Companies;
using Arslan.Vms.IdentityService.Users;
using Arslan.Vms.IdentityService.Files;
using Arslan.Vms.IdentityService.Addresses;
using Arslan.Vms.IdentityService.Addresses.AddressTypes;
using Arslan.Vms.IdentityService.Addresses.Version;

namespace Arslan.Vms.IdentityService.EntityFrameworkCore;

/* This is your actual DbContext used on runtime.
 * It includes only your entities.
 * It does not include entities of the used modules, because each module has already
 * its own DbContext class. If you want to share some database tables with the used modules,
 * just create a structure like done for AppUser.
 *
 * Don't use this DbContext for database migrations since it does not contain tables of the
 * used modules (as explained above). See AdministrationServiceMigrationsDbContext for migrations.
 */
[ConnectionStringName(AdministrationServiceDbProperties.ConnectionStringName)]
public class AdministrationServiceDbContext
    : AbpDbContext<AdministrationServiceDbContext>,
    IPermissionManagementDbContext,
    ISettingManagementDbContext,
    IAuditLoggingDbContext,
    IBlobStoringDbContext,
    IAdministrationServiceDbContext
{
    public AdministrationServiceDbContext(DbContextOptions<AdministrationServiceDbContext> options)
        : base(options)
    {

    }

    public DbSet<PermissionGrant> PermissionGrants { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<DatabaseBlobContainer> BlobContainers { get; set; }
    public DbSet<DatabaseBlob> Blobs { get; set; }

    #region Address
    public DbSet<Address> Address { get; set; }
    public DbSet<AddressType> AddressType { get; set; }
    public DbSet<AddressVersion> AddressVersion { get; set; }
    #endregion

    #region Company
    public DbSet<Company> Company { get; set; }
    public DbSet<CompanyAddress> CompanyAddress { get; set; }
    public DbSet<CompanyAttachment> CompanyAttachment { get; set; }
    #endregion

    #region Customer
    //public DbSet<CustomerPayment> CustomerPayment { get; set; }
    #endregion  

    #region Users
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRole { get; set; }
    public DbSet<UserAddress> UserAddress { get; set; }

    #endregion

    #region Attachment
    public DbSet<FileAttachment> FileAttachment { get; set; }
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureAdministrationService();

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureAuditLogging();
        builder.ConfigureBlobStoring();
    }
}