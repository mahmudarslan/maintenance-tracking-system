using Arslan.Vms.OrderService.PurchaseOrders;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.OrderService.v1.PurchaseOrders.Dtos
{
    public class CreatePurchaseOrderDto
    {
        public Guid VendorId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid TaxSchemeId { get; set; }
        public DateTime OrderDate { get; set; }
        public PurchaseOrderPaymentStatus PaymentStatus { get; set; }
        public PurchaseOrderInventoryStatus InventoryStatus { get; set; }
        public string OrderRemarks { get; set; }
        public decimal AmountPaid { get; set; }

        public List<CreatePurchaseOrderProductLineDto> ProductLines { get; set; }
        public List<CreatePurchaseOrderServiceLineDto> ServiceLines { get; set; }

        public CreatePurchaseOrderDto()
        {
            ProductLines = new List<CreatePurchaseOrderProductLineDto>();
            ServiceLines = new List<CreatePurchaseOrderServiceLineDto>();
        }
    }
}