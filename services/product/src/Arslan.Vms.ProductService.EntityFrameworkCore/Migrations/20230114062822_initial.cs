using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arslan.Vms.ProductService.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductServiceAcc_Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountType = table.Column<int>(type: "int", nullable: false),
                    Attr1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Attr2 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Sublocation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceAcc_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceAcc_Balances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastTransactionType = table.Column<int>(type: "int", nullable: false),
                    LastTransactionEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LastTransactionChildId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastTransactionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceAcc_Balances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceAcc_Checkpoints",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TransactionEntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionChildId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(22,5)", nullable: false),
                    BalanceAfter = table.Column<decimal>(type: "decimal(22,5)", nullable: false),
                    TransactionDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceAcc_Checkpoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceAddress",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DistrictId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddressType = table.Column<int>(type: "int", nullable: false),
                    AddressName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefaultAddress = table.Column<bool>(type: "bit", nullable: false),
                    IsBillingAddress = table.Column<bool>(type: "bit", nullable: false),
                    IsShippingAddress = table.Column<bool>(type: "bit", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceAddressTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceAddressTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceAddressVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DistrictId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AddressType = table.Column<int>(type: "int", nullable: false),
                    AddressName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefaultAddress = table.Column<bool>(type: "bit", nullable: false),
                    IsBillingAddress = table.Column<bool>(type: "bit", nullable: false),
                    IsShippingAddress = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceAddressVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceCurrencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Symbol = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    DecimalPlaces = table.Column<short>(type: "smallint", nullable: false),
                    DecimalSeparator = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    ThousandsSeparator = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    CRCurrencyPositionType = table.Column<short>(type: "smallint", nullable: false),
                    CRNegativeType = table.Column<short>(type: "smallint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceCurrencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceDocNoFormats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocNoType = table.Column<int>(type: "int", nullable: false),
                    NextNumber = table.Column<int>(type: "int", nullable: false),
                    MinDigits = table.Column<int>(type: "int", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceDocNoFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceFileAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DownloadGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UseDownloadUrl = table.Column<bool>(type: "bit", nullable: false),
                    DownloadUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNew = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceFileAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceProductPriceVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    PricingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemPriceType = table.Column<int>(type: "int", nullable: false),
                    FixedMarkup = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    StdUomPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PoUomPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceProductPriceVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceProductTaxCodeVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceProductTaxCodeVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceProductVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultSublocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReorderPoint = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ReorderQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Uom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MasterPackQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    InnerPackQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CaseLenght = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CaseWidth = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CaseHeight = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CaseWeight = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductLenght = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductWidth = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductHeight = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductWeight = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LastVendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSellable = table.Column<bool>(type: "bit", nullable: false),
                    IsPurchaseable = table.Column<bool>(type: "bit", nullable: false),
                    PictureFileAttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoUomName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoUomRatioStd = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SoUomRatio = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PoUomName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoUomRatioStd = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PoUomRatio = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceProductVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceSublocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceSublocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceTaxingSchemes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tax1Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tax2Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CalculateTax2OnTax1 = table.Column<bool>(type: "bit", nullable: false),
                    DefaultTaxCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceTaxingSchemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceVendorLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorItemCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cost = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceVendorLines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ProductCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultSublocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReorderPoint = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ReorderQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Uom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MasterPackQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    InnerPackQty = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CaseLenght = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CaseWidth = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CaseHeight = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    CaseWeight = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductLenght = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductWidth = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductHeight = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductWeight = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LastVendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSellable = table.Column<bool>(type: "bit", nullable: false),
                    IsPurchaseable = table.Column<bool>(type: "bit", nullable: false),
                    PictureFileAttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoUomName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoUomRatioStd = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SoUomRatio = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PoUomName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoUomRatioStd = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    PoUomRatio = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductServiceProducts_ProductServiceCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductServiceCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceCurrencyConversion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyConversionId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(22,5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceCurrencyConversion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductServiceCurrencyConversion_ProductServiceCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "ProductServiceCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductServicePricingSchemes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsTaxInclusive = table.Column<bool>(type: "bit", nullable: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServicePricingSchemes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductServicePricingSchemes_ProductServiceCurrencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "ProductServiceCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceTaxCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tax1Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Tax2Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceTaxCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductServiceTaxCodes_ProductServiceTaxingSchemes_TaxingSchemeId",
                        column: x => x.TaxingSchemeId,
                        principalTable: "ProductServiceTaxingSchemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceProductAttachments",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileAttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceProductAttachments", x => new { x.ProductId, x.FileAttachmentId });
                    table.ForeignKey(
                        name: "FK_ProductServiceProductAttachments_ProductServiceFileAttachments_FileAttachmentId",
                        column: x => x.FileAttachmentId,
                        principalTable: "ProductServiceFileAttachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductServiceProductAttachments_ProductServiceProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductServiceProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceProductTaxCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceProductTaxCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductServiceProductTaxCodes_ProductServiceProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductServiceProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductServiceProductTaxCodes_ProductServiceTaxingSchemes_TaxingSchemeId",
                        column: x => x.TaxingSchemeId,
                        principalTable: "ProductServiceTaxingSchemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductServiceProductPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    PricingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemPriceType = table.Column<int>(type: "int", nullable: false),
                    FixedMarkup = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    StdUomPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    PoUomPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductServiceProductPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductServiceProductPrices_ProductServicePricingSchemes_PricingSchemeId",
                        column: x => x.PricingSchemeId,
                        principalTable: "ProductServicePricingSchemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductServiceProductPrices_ProductServiceProducts_ProductId",
                        column: x => x.ProductId,
                        principalTable: "ProductServiceProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceCurrencyConversion_CurrencyId",
                table: "ProductServiceCurrencyConversion",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServicePricingSchemes_CurrencyId",
                table: "ProductServicePricingSchemes",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceProductAttachments_FileAttachmentId",
                table: "ProductServiceProductAttachments",
                column: "FileAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceProductPrices_PricingSchemeId",
                table: "ProductServiceProductPrices",
                column: "PricingSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceProductPrices_ProductId",
                table: "ProductServiceProductPrices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceProducts_ProductCategoryId",
                table: "ProductServiceProducts",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceProductTaxCodes_ProductId",
                table: "ProductServiceProductTaxCodes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceProductTaxCodes_TaxingSchemeId",
                table: "ProductServiceProductTaxCodes",
                column: "TaxingSchemeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductServiceTaxCodes_TaxingSchemeId",
                table: "ProductServiceTaxCodes",
                column: "TaxingSchemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductServiceAcc_Accounts");

            migrationBuilder.DropTable(
                name: "ProductServiceAcc_Balances");

            migrationBuilder.DropTable(
                name: "ProductServiceAcc_Checkpoints");

            migrationBuilder.DropTable(
                name: "ProductServiceAddress");

            migrationBuilder.DropTable(
                name: "ProductServiceAddressTypes");

            migrationBuilder.DropTable(
                name: "ProductServiceAddressVersions");

            migrationBuilder.DropTable(
                name: "ProductServiceCurrencyConversion");

            migrationBuilder.DropTable(
                name: "ProductServiceDocNoFormats");

            migrationBuilder.DropTable(
                name: "ProductServiceLocations");

            migrationBuilder.DropTable(
                name: "ProductServiceProductAttachments");

            migrationBuilder.DropTable(
                name: "ProductServiceProductPrices");

            migrationBuilder.DropTable(
                name: "ProductServiceProductPriceVersions");

            migrationBuilder.DropTable(
                name: "ProductServiceProductTaxCodes");

            migrationBuilder.DropTable(
                name: "ProductServiceProductTaxCodeVersions");

            migrationBuilder.DropTable(
                name: "ProductServiceProductVersions");

            migrationBuilder.DropTable(
                name: "ProductServiceSublocations");

            migrationBuilder.DropTable(
                name: "ProductServiceTaxCodes");

            migrationBuilder.DropTable(
                name: "ProductServiceVendorLines");

            migrationBuilder.DropTable(
                name: "ProductServiceFileAttachments");

            migrationBuilder.DropTable(
                name: "ProductServicePricingSchemes");

            migrationBuilder.DropTable(
                name: "ProductServiceProducts");

            migrationBuilder.DropTable(
                name: "ProductServiceTaxingSchemes");

            migrationBuilder.DropTable(
                name: "ProductServiceCurrencies");

            migrationBuilder.DropTable(
                name: "ProductServiceCategories");
        }
    }
}
