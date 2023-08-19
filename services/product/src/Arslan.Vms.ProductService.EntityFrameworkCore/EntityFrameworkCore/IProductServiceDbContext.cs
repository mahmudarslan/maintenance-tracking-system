using Arslan.Vms.ProductService.Currencies;
using Arslan.Vms.ProductService.Locations;
using Arslan.Vms.ProductService.Products.Versions;
using Arslan.Vms.ProductService.Products;
using Arslan.Vms.ProductService.Taxes;
using Arslan.Vms.ProductService.Vendors;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Arslan.Vms.ProductService.Files;
using Arslan.Vms.ProductService.Categories;
using Arslan.Vms.ProductService.Addresses.Version;
using Arslan.Vms.ProductService.Addresses.AddressTypes;
using Arslan.Vms.ProductService.Addresses;
using Arslan.Vms.ProductService.Pricing;
using Arslan.Vms.ProductService.DocumentNoFormats;
using Arslan.Vms.ProductService.Accounts;

namespace Arslan.Vms.ProductService.EntityFrameworkCore;

[ConnectionStringName(ProductServiceDbProperties.ConnectionStringName)]
public interface IProductServiceDbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */

    #region Address
    public DbSet<Address> Address { get; set; }
    public DbSet<AddressType> AddressType { get; set; }
    public DbSet<AddressVersion> AddressVersion { get; set; }
    #endregion   

    #region Vendor
    public DbSet<VendorLine> VendorLine { get; set; }
    //public DbSet<VendorPayment> VendorPayment { get; set; }
    #endregion     

    #region Product
    DbSet<Product> Product { get; set; }
    DbSet<ProductAttachment> ProductAttachment { get; set; }
    DbSet<Category> ProductCategory { get; set; }
    DbSet<ProductPrice> ProductPrice { get; set; }
    DbSet<ProductPriceVersion> ProductPriceVersion { get; set; }
    DbSet<ProductTaxCode> ProductTaxCode { get; set; }
    DbSet<ProductTaxCodeVersion> ProductTaxCodeVersion { get; set; }
    DbSet<ProductVersion> ProductVersion { get; set; }
    #endregion

    #region Pricing   
    DbSet<PricingScheme> PricingScheme { get; set; }
    #endregion

    #region Currencies
    DbSet<Currency> Currency { get; set; }
    public DbSet<CurrencyConversion> CurrencyConversion { get; set; }
    #endregion

    #region Taxes
    DbSet<TaxingScheme> TaxingScheme { get; set; }
    DbSet<TaxCode> TaxCode { get; set; }
    #endregion

    #region Location
    DbSet<Location> Location { get; set; }
    DbSet<Sublocation> Sublocation { get; set; }
    #endregion

    #region Attachment
    public DbSet<FileAttachment> FileAttachment { get; set; }
    #endregion

    #region DocNoFormat
    public DbSet<DocNoFormat> DocNoFormat { get; set; }
    #endregion

    #region Account
    DbSet<Account> Account { get; set; }
    DbSet<Balances> Balances { get; set; }
    DbSet<Checkpoint> Checkpoint { get; set; }
    #endregion
}
