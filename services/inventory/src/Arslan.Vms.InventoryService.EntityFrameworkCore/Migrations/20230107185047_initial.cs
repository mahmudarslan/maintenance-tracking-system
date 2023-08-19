using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Arslan.Vms.InventoryService.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    DownloadGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UseDownloadUrl = table.Column<bool>(type: "bit", nullable: false),
                    DownloadUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNew = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileAttachment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvDocNoFormat",
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
                    table.PrimaryKey("PK_InvDocNoFormat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryStockAdjustmentLineVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockAdjustmentLineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    StockAdjustmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sublocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QuantityBefore = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QuantityBeforeUom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    QuantityBeforeDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QuantityAfter = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QuantityAfterUom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    QuantityAfterDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Difference = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DifferenceUom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DifferenceDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryStockAdjustmentLineVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryStockAdjustments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    AdjustmentNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_InventoryStockAdjustments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryStockAdjustmentVersions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StockAdjustmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    AdjustmentNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_InventoryStockAdjustmentVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryStockAdjustmentAttachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StockAdjustmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileAttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryStockAdjustmentAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryStockAdjustmentAttachments_InventoryStockAdjustments_StockAdjustmentId",
                        column: x => x.StockAdjustmentId,
                        principalTable: "InventoryStockAdjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryStockAdjustmentLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<int>(type: "int", nullable: false),
                    StockAdjustmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Sublocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QuantityBefore = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QuantityBeforeUom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    QuantityBeforeDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QuantityAfter = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    QuantityAfterUom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    QuantityAfterDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Difference = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    DifferenceUom = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    DifferenceDisplay = table.Column<decimal>(type: "decimal(18,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryStockAdjustmentLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryStockAdjustmentLines_InventoryStockAdjustments_StockAdjustmentId",
                        column: x => x.StockAdjustmentId,
                        principalTable: "InventoryStockAdjustments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStockAdjustmentAttachments_StockAdjustmentId",
                table: "InventoryStockAdjustmentAttachments",
                column: "StockAdjustmentId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStockAdjustmentLines_StockAdjustmentId",
                table: "InventoryStockAdjustmentLines",
                column: "StockAdjustmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileAttachment");

            migrationBuilder.DropTable(
                name: "InvDocNoFormat");

            migrationBuilder.DropTable(
                name: "InventoryStockAdjustmentAttachments");

            migrationBuilder.DropTable(
                name: "InventoryStockAdjustmentLines");

            migrationBuilder.DropTable(
                name: "InventoryStockAdjustmentLineVersions");

            migrationBuilder.DropTable(
                name: "InventoryStockAdjustmentVersions");

            migrationBuilder.DropTable(
                name: "InventoryStockAdjustments");
        }
    }
}
