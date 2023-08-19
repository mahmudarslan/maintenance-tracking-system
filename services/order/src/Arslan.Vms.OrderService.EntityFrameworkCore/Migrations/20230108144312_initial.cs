using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arslan.Vms.OrderService.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderAttachments",
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
                    table.PrimaryKey("PK_OrderAttachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderOrderDocNoFormats",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DocNoType = table.Column<int>(type: "int", nullable: false),
                    NextNumber = table.Column<int>(type: "int", nullable: false),
                    MinDigits = table.Column<int>(type: "int", nullable: false),
                    Prefix = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Suffix = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderOrderDocNoFormats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ProductType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DefaultLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultSublocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReorderPoint = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReorderQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Uom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MasterPackQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InnerPackQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CaseLenght = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CaseWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CaseHeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CaseWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductLenght = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductWidth = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductHeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastVendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsSellable = table.Column<bool>(type: "bit", nullable: false),
                    IsPurchaseable = table.Column<bool>(type: "bit", nullable: false),
                    PictureFileAttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SoUomName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SoUomRatioStd = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoUomRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PoUomName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PoUomRatioStd = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PoUomRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_OrderProduct", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderPurchaseOrderLineVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorLineCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    QuantityUom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    DiscountIsPercent = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNum = table.Column<int>(type: "int", nullable: false),
                    TaxCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tax1Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Tax2Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    ServiceCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPurchaseOrderLineVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderPurchaseOrderReceiveLineVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderReceiveLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ReceiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorLineCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sublocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityUom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPurchaseOrderReceiveLineVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderPurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    VendorOrderNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderSubTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTax1 = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTax2 = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderExtra = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    TaxingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tax1Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Tax2Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    CalculateTax2OnTax1 = table.Column<bool>(type: "bit", nullable: false),
                    Tax1Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tax2Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AmountPaid = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    AncillaryExpenses = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    AncillaryIsPercent = table.Column<bool>(type: "bit", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SummaryLinePermutation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiveRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    InventoryStatus = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsTaxInclusive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_OrderPurchaseOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderPurchaseOrderVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    VendorOrderNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Carrier = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VendorAddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderSubTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTax1 = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTax2 = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderExtra = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    Tax1Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Tax2Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    CalculateTax2OnTax1 = table.Column<bool>(type: "bit", nullable: false),
                    Tax1Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tax2Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReceiveRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    UnstockRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AncillaryExpenses = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    AncillaryIsPercent = table.Column<bool>(type: "bit", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    SummaryLinePermutation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    InventoryStatus = table.Column<int>(type: "int", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsTaxInclusive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_OrderPurchaseOrderVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderSalesOrderLineVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesOrderLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    LineNum = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    QuantityUom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    QuantityDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DiscountIsPercent = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Tax1Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Tax2Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    ServiceCompleted = table.Column<bool>(type: "bit", nullable: true),
                    TechnicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSalesOrderLineVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderSalesOrderPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_OrderSalesOrderPayments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderSalesOrderPickLineVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesOrderPickLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    LineNum = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sublocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QuantityUom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    QuantityDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PickDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSalesOrderPickLineVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderSalesOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentSalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PricingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    InventoryStatus = table.Column<int>(type: "int", nullable: false),
                    HeadTechnicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkorderTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VehicleReceiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kilometrage = table.Column<int>(type: "int", nullable: false),
                    VehicleReceiveFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesRepresentative = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PONumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoicedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderSubTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTax1 = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTax2 = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderExtra = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    Tax1Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Tax2Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Tax1Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tax2Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NonCustomerCost = table.Column<decimal>(type: "decimal(21,5)", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    PickingRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryLinePermutation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NonCustomerCostIsPercent = table.Column<bool>(type: "bit", nullable: false),
                    CalculateTax2OnTax1 = table.Column<bool>(type: "bit", nullable: false),
                    IsQuote = table.Column<bool>(type: "bit", nullable: false),
                    IsInvoiced = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsTaxInclusive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_OrderSalesOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderSalesOrderVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentSalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentTermId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PricingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    InventoryStatus = table.Column<int>(type: "int", nullable: false),
                    HeadTechnicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkorderTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VehicleReceiveDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kilometrage = table.Column<int>(type: "int", nullable: false),
                    VehicleReceiveFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesRepresentative = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PONumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContactName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PickedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoicedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderSubTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTax1 = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTax2 = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderExtra = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    OrderTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    Tax1Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Tax2Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Tax1Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Tax2Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    NonCustomerCost = table.Column<decimal>(type: "decimal(21,5)", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(20,10)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    PickingRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SummaryLinePermutation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NonCustomerCostIsPercent = table.Column<bool>(type: "bit", nullable: false),
                    CalculateTax2OnTax1 = table.Column<bool>(type: "bit", nullable: false),
                    IsQuote = table.Column<bool>(type: "bit", nullable: false),
                    IsInvoiced = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsTaxInclusive = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_OrderSalesOrderVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderWorkorderTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderWorkorderTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxingSchemes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tax1Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tax2Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_TaxingSchemes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderPurchaseOrderAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileAttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPurchaseOrderAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPurchaseOrderAttachments_OrderPurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "OrderPurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderPurchaseOrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNum = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorLineCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    QuantityUom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,5)", nullable: false),
                    DiscountIsPercent = table.Column<bool>(type: "bit", nullable: false),
                    TaxCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tax1Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Tax2Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    ServiceCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPurchaseOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPurchaseOrderLines_OrderPurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "OrderPurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderPurchaseOrderPayments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    table.PrimaryKey("PK_OrderPurchaseOrderPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPurchaseOrderPayments_OrderPurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "OrderPurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderPurchaseOrderReceiveLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    ReceiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorLineCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sublocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityUom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LineNum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderPurchaseOrderReceiveLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderPurchaseOrderReceiveLines_OrderPurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "OrderPurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderSalesOrderAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileAttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSalesOrderAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderSalesOrderAttachments_OrderSalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "OrderSalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderSalesOrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxCodeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    LineNum = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(21,5)", nullable: false),
                    QuantityUom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    QuantityDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DiscountIsPercent = table.Column<bool>(type: "bit", nullable: false),
                    Tax1Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    Tax2Rate = table.Column<decimal>(type: "decimal(10,5)", nullable: false),
                    ServiceCompleted = table.Column<bool>(type: "bit", nullable: true),
                    TechnicianId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSalesOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderSalesOrderLines_OrderSalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "OrderSalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderSalesOrderPickLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SalesOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    LineNum = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sublocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QuantityUom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    QuantityDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PickDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSalesOrderPickLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderSalesOrderPickLines_OrderSalesOrders_SalesOrderId",
                        column: x => x.SalesOrderId,
                        principalTable: "OrderSalesOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaxCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TaxingSchemeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tax1Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax2Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaxCodes_TaxingSchemes_TaxingSchemeId",
                        column: x => x.TaxingSchemeId,
                        principalTable: "TaxingSchemes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderPurchaseOrderAttachments_PurchaseOrderId",
                table: "OrderPurchaseOrderAttachments",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPurchaseOrderLines_PurchaseOrderId",
                table: "OrderPurchaseOrderLines",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderPurchaseOrderPayments_PurchaseOrderId",
                table: "OrderPurchaseOrderPayments",
                column: "PurchaseOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderPurchaseOrderReceiveLines_PurchaseOrderId",
                table: "OrderPurchaseOrderReceiveLines",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSalesOrderAttachments_SalesOrderId",
                table: "OrderSalesOrderAttachments",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSalesOrderLines_SalesOrderId",
                table: "OrderSalesOrderLines",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSalesOrderPickLines_SalesOrderId",
                table: "OrderSalesOrderPickLines",
                column: "SalesOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TaxCodes_TaxingSchemeId",
                table: "TaxCodes",
                column: "TaxingSchemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderAttachments");

            migrationBuilder.DropTable(
                name: "OrderOrderDocNoFormats");

            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.DropTable(
                name: "OrderPurchaseOrderAttachments");

            migrationBuilder.DropTable(
                name: "OrderPurchaseOrderLines");

            migrationBuilder.DropTable(
                name: "OrderPurchaseOrderLineVersions");

            migrationBuilder.DropTable(
                name: "OrderPurchaseOrderPayments");

            migrationBuilder.DropTable(
                name: "OrderPurchaseOrderReceiveLines");

            migrationBuilder.DropTable(
                name: "OrderPurchaseOrderReceiveLineVersions");

            migrationBuilder.DropTable(
                name: "OrderPurchaseOrderVersions");

            migrationBuilder.DropTable(
                name: "OrderSalesOrderAttachments");

            migrationBuilder.DropTable(
                name: "OrderSalesOrderLines");

            migrationBuilder.DropTable(
                name: "OrderSalesOrderLineVersions");

            migrationBuilder.DropTable(
                name: "OrderSalesOrderPayments");

            migrationBuilder.DropTable(
                name: "OrderSalesOrderPickLines");

            migrationBuilder.DropTable(
                name: "OrderSalesOrderPickLineVersions");

            migrationBuilder.DropTable(
                name: "OrderSalesOrderVersions");

            migrationBuilder.DropTable(
                name: "OrderWorkorderTypes");

            migrationBuilder.DropTable(
                name: "TaxCodes");

            migrationBuilder.DropTable(
                name: "OrderPurchaseOrders");

            migrationBuilder.DropTable(
                name: "OrderSalesOrders");

            migrationBuilder.DropTable(
                name: "TaxingSchemes");
        }
    }
}
