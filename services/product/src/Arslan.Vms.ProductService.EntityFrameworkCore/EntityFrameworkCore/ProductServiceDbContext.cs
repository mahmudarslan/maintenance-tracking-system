using Arslan.Vms.ProductService.Currencies;
using Arslan.Vms.ProductService.Locations;
using Arslan.Vms.ProductService.Products.Versions;
using Arslan.Vms.ProductService.Products;
using Arslan.Vms.ProductService.Taxes;
using Arslan.Vms.ProductService.Vendors;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Arslan.Vms.ProductService.Categories;
using Arslan.Vms.ProductService.Addresses;
using Arslan.Vms.ProductService.Addresses.AddressTypes;
using Arslan.Vms.ProductService.Addresses.Version;
using Arslan.Vms.ProductService.Pricing;
using Arslan.Vms.ProductService.Files;
using Arslan.Vms.ProductService.DocumentNoFormats;
using Arslan.Vms.ProductService.Accounts;

namespace Arslan.Vms.ProductService.EntityFrameworkCore;

[ConnectionStringName(ProductServiceDbProperties.ConnectionStringName)]
public class ProductServiceDbContext : AbpDbContext<ProductServiceDbContext>, IProductServiceDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    #region Address
    public DbSet<Address> Address { get; set; }
    public DbSet<AddressType> AddressType { get; set; }
    public DbSet<AddressVersion> AddressVersion { get; set; }
    #endregion 

    #region Customer
    //public DbSet<CustomerPayment> CustomerPayment { get; set; }
    #endregion

    #region Vendor
    public DbSet<VendorLine> VendorLine { get; set; }
    //public DbSet<VendorPayment> VendorPayment { get; set; }
    #endregion
     

    #region Product
    public DbSet<Product> Product { get; set; }
    public DbSet<ProductAttachment> ProductAttachment { get; set; }
    public DbSet<Category> ProductCategory { get; set; }
    public DbSet<ProductPrice> ProductPrice { get; set; }
    public DbSet<ProductPriceVersion> ProductPriceVersion { get; set; }
    public DbSet<ProductTaxCode> ProductTaxCode { get; set; }
    public DbSet<ProductTaxCodeVersion> ProductTaxCodeVersion { get; set; }
    public DbSet<ProductVersion> ProductVersion { get; set; }
    #endregion

    #region Pricing  
    public DbSet<PricingScheme> PricingScheme { get; set; }
    #endregion

    #region Currencies
    public DbSet<Currency> Currency { get; set; }
    public DbSet<CurrencyConversion> CurrencyConversion { get; set; }
    #endregion

    #region Taxes
    public DbSet<TaxingScheme> TaxingScheme { get; set; }
    public DbSet<TaxCode> TaxCode { get; set; }
    #endregion

    #region Location
    public DbSet<Location> Location { get; set; }
    public DbSet<Sublocation> Sublocation { get; set; }
    #endregion

    #region Attachment
    public DbSet<FileAttachment> FileAttachment { get; set; }
    #endregion

    #region DocNoFormat
    public DbSet<DocNoFormat> DocNoFormat { get; set; }
    #endregion

    #region Account
    public DbSet<Account> Account { get; set; }
    public DbSet<Balances> Balances { get; set; }
    public DbSet<Checkpoint> Checkpoint { get; set; }
    #endregion

    public ProductServiceDbContext(DbContextOptions<ProductServiceDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureProductService();
    }
}
