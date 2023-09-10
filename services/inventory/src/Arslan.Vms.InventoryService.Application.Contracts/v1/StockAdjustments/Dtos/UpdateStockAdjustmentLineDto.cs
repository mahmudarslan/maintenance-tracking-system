using System;
using Volo.Abp.Application.Dtos;

namespace Arslan.Vms.InventoryService.v1.StockAdjustments.Dtos
{
    public class UpdateStockAdjustmentLineDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Description { get; set; }
        public Guid LocationId { get; set; }
        public Guid? SublocationId { get; set; }
        public decimal QuantityBefore { get; set; }
        public decimal QuantityAfter { get; set; }
        public decimal Difference { get; set; }
        //public string Serials { get; set; }
        public bool IsDeleted { get; set; }
    }
}