using System;

namespace Arslan.Vms.InventoryService.v1.StockAdjustments.Dtos
{
    public class StockAdjustmentLineDto
    {
        public Guid Id { get; set; }
        public Guid? StockAdjustmentId { get; set; }
        public Guid? ProductId { get; set; }
        public Guid LocationId { get; set; }
        public string Description { get; set; }
        public decimal QuantityBefore { get; set; }
        public decimal QuantityAfter { get; set; }
        public decimal Difference { get; set; }
        //public bool IsDeleted { get; set; }
    }
}