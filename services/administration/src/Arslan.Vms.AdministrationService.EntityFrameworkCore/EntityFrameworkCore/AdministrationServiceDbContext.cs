using Arslan.Vms.AdministrationService.Saas;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DistributedEvents;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.PermissionManagement;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace Arslan.Vms.AdministrationService.EntityFrameworkCore;

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
		IFeatureManagementDbContext,
		//IBlobStoringDbContext,
		ITenantManagementDbContext,
		IHasEventOutbox
{
    public AdministrationServiceDbContext(DbContextOptions<AdministrationServiceDbContext> options)
        : base(options)
    {

    }

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */
    public DbSet<PermissionGroupDefinitionRecord> PermissionGroups { get; set; }
    public DbSet<PermissionDefinitionRecord> Permissions { get; set; }
    public DbSet<PermissionGrant> PermissionGrants { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<FeatureGroupDefinitionRecord> FeatureGroups { get; set; }
    public DbSet<FeatureDefinitionRecord> Features { get; set; }
    public DbSet<FeatureValue> FeatureValues { get; set; }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }
    public DbSet<OutgoingEventRecord> OutgoingEvents { get; set; }
    public DbSet<IncomingEventRecord> IncomingEvents { get; set; }
    #endregion

    public DbSet<SaasEdition> SaasEditions { get; set; }

 

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

		builder.ConfigurePermissionManagement();
		builder.ConfigureSettingManagement();
		//builder.ConfigureBackgroundJobs();
		builder.ConfigureAuditLogging();
		builder.ConfigureFeatureManagement();
		builder.ConfigureTenantManagement();
		builder.ConfigureEventOutbox();

        builder.Entity<SaasEdition>(b =>
        {
            b.ToTable(AdministrationServiceDbProperties.DbTablePrefix + "SaasEditions", AdministrationServiceDbProperties.DbSchema);

            b.ConfigureFullAuditedAggregateRoot();

            b.Property(t => t.Name).IsRequired().HasMaxLength(SaasEditionMaxNameLengthConsts.MaxNameLength);

            b.HasIndex(u => u.Name);
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableDetailedErrors().EnableSensitiveDataLogging();

        base.OnConfiguring(optionsBuilder);
    }
}