using System;
using System.Collections.Generic;

namespace Arslan.Vms.InventoryService.v1.StockAdjustments.Dtos
{
    public class CreateStockAdjustmentDto
    {
        public string AdjustmentNumber { get; set; }
        public string Remarks { get; set; }
        public List<CreateStockAdjustmentLineDto> Lines { get; set; }

        public CreateStockAdjustmentDto()
        {
            Lines = new List<CreateStockAdjustmentLineDto>();
        }

    }
}