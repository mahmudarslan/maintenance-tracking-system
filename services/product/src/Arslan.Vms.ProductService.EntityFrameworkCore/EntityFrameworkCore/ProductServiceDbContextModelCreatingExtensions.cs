using Arslan.Vms.ProductService.Accounts;
using Arslan.Vms.ProductService.Addresses.Version;
using Arslan.Vms.ProductService.Currencies;
using Arslan.Vms.ProductService.DocumentNoFormats;
using Arslan.Vms.ProductService.Files;
using Arslan.Vms.ProductService.Locations;
using Arslan.Vms.ProductService.Products.Versions;
using Arslan.Vms.ProductService.Products;
using Arslan.Vms.ProductService.Taxes;
using Arslan.Vms.ProductService.Vendors;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Arslan.Vms.ProductService.Categories;
using Arslan.Vms.ProductService.Pricing;
using Arslan.Vms.ProductService.Addresses.AddressTypes;
using Arslan.Vms.ProductService.Addresses;

namespace Arslan.Vms.ProductService.EntityFrameworkCore;

public static class ProductServiceDbContextModelCreatingExtensions
{
    public static void ConfigureProductService(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "Questions", ProductServiceDbProperties.DbSchema);

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
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "Address", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<AddressVersion>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "AddressVersions", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        #endregion

        #region Address Type
        builder.Entity<AddressType>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "AddressTypes", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Name).HasMaxLength(AddressTypeConsts.NameMaxLength);
        });
        #endregion



        #region Vendor
        builder.Entity<VendorLine>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "VendorLines", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Cost).HasColumnType("decimal(18,5)");
            b.Property(x => x.VendorItemCode).HasMaxLength(VendorLineConsts.VendorItemCodeMaxLength);
        });
        #endregion

        #region Product
        builder.Entity<Product>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "Products", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.HasOne<Category>().WithMany().HasForeignKey(h => h.ProductCategoryId);
            b.Property(x => x.ReorderPoint).HasColumnType("decimal(18,4)");
            b.Property(x => x.ReorderQuantity).HasColumnType("decimal(18,4)");
            b.Property(x => x.MasterPackQty).HasColumnType("decimal(18,4)");
            b.Property(x => x.InnerPackQty).HasColumnType("decimal(18,4)");
            b.Property(x => x.CaseLenght).HasColumnType("decimal(18,4)");
            b.Property(x => x.CaseWidth).HasColumnType("decimal(18,4)");
            b.Property(x => x.CaseHeight).HasColumnType("decimal(18,4)");
            b.Property(x => x.CaseWeight).HasColumnType("decimal(18,4)");
            b.Property(x => x.ProductLenght).HasColumnType("decimal(18,4)");
            b.Property(x => x.ProductWidth).HasColumnType("decimal(18,4)");
            b.Property(x => x.ProductHeight).HasColumnType("decimal(18,4)");
            b.Property(x => x.ProductWeight).HasColumnType("decimal(18,4)");
            b.Property(x => x.SoUomRatioStd).HasColumnType("decimal(18,4)");
            b.Property(x => x.SoUomRatio).HasColumnType("decimal(18,4)");
            b.Property(x => x.PoUomRatioStd).HasColumnType("decimal(18,4)");
            b.Property(x => x.PoUomRatio).HasColumnType("decimal(18,4)");
            b.Property(x => x.UnitCost).HasColumnType("decimal(18,4)");
            b.Property(x => x.Name).HasMaxLength(ProductConsts.NameMaxLength).IsRequired();
            b.Property(x => x.Barcode).HasMaxLength(ProductConsts.BarcodeMaxLength);
            b.Property(x => x.DefaultSublocation).HasMaxLength(ProductConsts.DefaultSublocationMaxLength);
            b.Property(x => x.Uom).HasMaxLength(ProductConsts.UomMaxLength);
            b.Property(x => x.ProductCategoryId).IsRequired();
            b.Property(x => x.ProductType).IsRequired();
        });
        builder.Entity<ProductAttachment>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "ProductAttachments", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.HasKey(ur => new { ur.ProductId, ur.FileAttachmentId });
            b.HasOne<FileAttachment>().WithMany().HasForeignKey(h => h.FileAttachmentId);
        });
        builder.Entity<ProductPrice>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "ProductPrices", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<PricingScheme>().WithMany().HasForeignKey(h => h.PricingSchemeId);
            b.Property(x => x.UnitPrice).HasColumnType("decimal(18,5)");
            b.Property(x => x.FixedMarkup).HasColumnType("decimal(18,5)");
            b.Property(x => x.StdUomPrice).HasColumnType("decimal(18,5)");
            b.Property(x => x.PoUomPrice).HasColumnType("decimal(18,5)");
        });
        builder.Entity<ProductTaxCode>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "ProductTaxCodes", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<TaxingScheme>().WithMany().HasForeignKey(h => h.TaxingSchemeId);
        });
        builder.Entity<ProductPriceVersion>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "ProductPriceVersions", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.UnitPrice).HasColumnType("decimal(18,5)");
            b.Property(x => x.FixedMarkup).HasColumnType("decimal(18,5)");
            b.Property(x => x.StdUomPrice).HasColumnType("decimal(18,5)");
            b.Property(x => x.PoUomPrice).HasColumnType("decimal(18,5)");
        });
        builder.Entity<ProductTaxCodeVersion>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "ProductTaxCodeVersions", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<ProductVersion>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "ProductVersions", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.ReorderPoint).HasColumnType("decimal(18,4)");
            b.Property(x => x.ReorderQuantity).HasColumnType("decimal(18,4)");
            b.Property(x => x.MasterPackQty).HasColumnType("decimal(18,4)");
            b.Property(x => x.InnerPackQty).HasColumnType("decimal(18,4)");
            b.Property(x => x.CaseLenght).HasColumnType("decimal(18,4)");
            b.Property(x => x.CaseWidth).HasColumnType("decimal(18,4)");
            b.Property(x => x.CaseHeight).HasColumnType("decimal(18,4)");
            b.Property(x => x.CaseWeight).HasColumnType("decimal(18,4)");
            b.Property(x => x.ProductLenght).HasColumnType("decimal(18,4)");
            b.Property(x => x.ProductWidth).HasColumnType("decimal(18,4)");
            b.Property(x => x.ProductHeight).HasColumnType("decimal(18,4)");
            b.Property(x => x.ProductWeight).HasColumnType("decimal(18,4)");
            b.Property(x => x.SoUomRatioStd).HasColumnType("decimal(18,4)");
            b.Property(x => x.SoUomRatio).HasColumnType("decimal(18,4)");
            b.Property(x => x.PoUomRatioStd).HasColumnType("decimal(18,4)");
            b.Property(x => x.PoUomRatio).HasColumnType("decimal(18,4)");
            b.Property(x => x.UnitCost).HasColumnType("decimal(18,4)");
            b.Property(x => x.Name).HasMaxLength(ProductConsts.NameMaxLength).IsRequired();
            b.Property(x => x.Barcode).HasMaxLength(ProductConsts.BarcodeMaxLength);
            b.Property(x => x.DefaultSublocation).HasMaxLength(ProductConsts.DefaultSublocationMaxLength);
            b.Property(x => x.Uom).HasMaxLength(ProductConsts.UomMaxLength);
            b.Property(x => x.ProductCategoryId).IsRequired();
            b.Property(x => x.ProductType).IsRequired();
        });
        #endregion

        #region Category
        builder.Entity<Category>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "Categories", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Name).IsRequired();
        });
        #endregion

        #region Location
        builder.Entity<Location>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "Locations", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Name).HasMaxLength(LocationConsts.NameMaxLength);
        });
        builder.Entity<Sublocation>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "Sublocations", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        #endregion

        #region Price
        builder.Entity<PricingScheme>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "PricingSchemes", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<Currency>().WithMany().HasForeignKey(h => h.CurrencyId);
            b.Property(x => x.Name).HasMaxLength(PricingSchemeConsts.NameMaxLength);
        });
        #endregion

        #region Currencies
        builder.Entity<CurrencyConversion>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "CurrencyConversion", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.HasOne<Currency>().WithMany().HasForeignKey(h => h.CurrencyId);
            b.Property(x => x.ExchangeRate).HasColumnType("decimal(22,5)");
        });
        builder.Entity<Currency>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "Currencies", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Code).HasMaxLength(CurrencyConsts.CodeMaxLength);
            b.Property(x => x.Description).HasMaxLength(CurrencyConsts.DescriptionMaxLength);
            b.Property(x => x.Symbol).HasMaxLength(CurrencyConsts.SymbolMaxLength);
            b.Property(x => x.DecimalSeparator).HasMaxLength(CurrencyConsts.DecimalSeparatorMaxLength);
            b.Property(x => x.ThousandsSeparator).HasMaxLength(CurrencyConsts.ThousandsSeparatorMaxLength);

        });
        #endregion

        #region Taxes
        builder.Entity<TaxingScheme>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "TaxingSchemes", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Name).HasMaxLength(TaxingSchemeConsts.NameMaxLength);
            b.Property(x => x.Tax1Name).HasMaxLength(TaxingSchemeConsts.Tax1NameMaxLength);
            b.Property(x => x.Tax2Name).HasMaxLength(TaxingSchemeConsts.Tax2NameMaxLength);
        });
        builder.Entity<TaxCode>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "TaxCodes", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.Tax1Rate).HasColumnType("decimal(10,5)");
            b.Property(x => x.Tax2Rate).HasColumnType("decimal(10,5)");
            b.Property(x => x.Name).HasMaxLength(TaxCodeConsts.NameMaxLength);
        });
        #endregion

        #region DocNoFormats
        builder.Entity<DocNoFormat>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "DocNoFormats", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Prefix).HasMaxLength(DocNoFormatConsts.PrefixMaxLength).IsRequired();
            b.Property(x => x.Suffix).HasMaxLength(DocNoFormatConsts.SuffixMaxLength).IsRequired();
        });
        #endregion

        #region Attachment
        builder.Entity<FileAttachment>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "FileAttachments", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        #endregion

        #region Account
        builder.Entity<Account>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "Acc_Accounts", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
        });
        builder.Entity<Balances>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "Acc_Balances", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Balance).HasColumnType("decimal(18,4)");
        });
        builder.Entity<Checkpoint>(b =>
        {
            b.ToTable(ProductServiceDbProperties.DbTablePrefix + "Acc_Checkpoints", ProductServiceDbProperties.DbSchema);
            b.ConfigureByConvention();
            b.ConfigureFullAuditedAggregateRoot();
            b.Property(x => x.Amount).HasColumnType("decimal(22,5)");
            b.Property(x => x.BalanceAfter).HasColumnType("decimal(22,5)");
        });
        #endregion
    }
}
