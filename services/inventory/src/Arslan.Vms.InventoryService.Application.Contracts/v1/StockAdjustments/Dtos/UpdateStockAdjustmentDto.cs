using System;
using System.Collections.Generic;

namespace Arslan.Vms.InventoryService.v1.StockAdjustments.Dtos
{
    public class UpdateStockAdjustmentDto
    {
        public string AdjustmentNumber { get; set; }
        public string Remarks { get; set; }
        public bool IsCancelled { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<UpdateStockAdjustmentLineDto> Lines { get; set; }

        public UpdateStockAdjustmentDto()
        {
            Lines = new List<UpdateStockAdjustmentLineDto>();
        }

    }
}