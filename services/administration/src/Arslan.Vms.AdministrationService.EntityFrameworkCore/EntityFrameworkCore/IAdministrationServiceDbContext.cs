using Arslan.Vms.AdministrationService.Addresses;
using Arslan.Vms.AdministrationService.Addresses.AddressTypes;
using Arslan.Vms.AdministrationService.Addresses.Version;
using Arslan.Vms.AdministrationService.Companies;
using Arslan.Vms.AdministrationService.Files;
using Arslan.Vms.AdministrationService.Users;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.AdministrationService.EntityFrameworkCore;

[ConnectionStringName(AdministrationServiceDbProperties.ConnectionStringName)]
public interface IAdministrationServiceDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
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
}
