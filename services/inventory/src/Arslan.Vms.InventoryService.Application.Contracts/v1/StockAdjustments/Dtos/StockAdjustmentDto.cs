using System;
using System.Collections.Generic;

namespace Arslan.Vms.InventoryService.v1.StockAdjustments.Dtos
{
    public class StockAdjustmentDto
    {
        public Guid Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string AdjustmentNumber { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsDeleted { get; set; }
        public List<StockAdjustmentLineDto> Lines { get; set; }

        public StockAdjustmentDto()
        {
            Lines = new List<StockAdjustmentLineDto>();
        }
    }
}