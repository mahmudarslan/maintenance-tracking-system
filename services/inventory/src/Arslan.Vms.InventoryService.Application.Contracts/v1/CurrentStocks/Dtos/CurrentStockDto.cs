using System;

namespace Arslan.Vms.InventoryService.v1.CurrentStocks.Dtos
{
    public class CurrentStockDto
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Guid ProductCategoryId { get; set; }
        public virtual Guid LocationId { get; set; }
        public virtual decimal Quantity { get; set; }
    }
}