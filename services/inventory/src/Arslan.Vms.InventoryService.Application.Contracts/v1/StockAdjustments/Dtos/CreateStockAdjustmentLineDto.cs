using System;
using Volo.Abp.Application.Dtos;

namespace Arslan.Vms.InventoryService.v1.StockAdjustments.Dtos
{
    public class CreateStockAdjustmentLineDto
    {
        public Guid ProductId { get; set; }
        public string Description { get; set; }
        public Guid LocationId { get; set; }
        public Guid? SublocationId { get; set; }
        public decimal QuantityBefore { get; set; }
        public decimal QuantityAfter { get; set; }
        public decimal Difference { get; set; }
    }
}