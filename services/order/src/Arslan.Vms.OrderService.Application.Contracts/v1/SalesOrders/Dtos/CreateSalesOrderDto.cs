using Arslan.Vms.OrderService.SalseOrders;
using Arslan.Vms.OrderService.v1.Files.Dtos;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.OrderService.v1.SalesOrders.Dtos
{
    public class CreateSalesOrderDto
    {
        public Guid CustomerId { get; set; }
        public Guid? HeadTechnicianId { get; set; }
        public Guid? VehicleId { get; set; }
        public Guid? WorkorderTypeId { get; set; }
        public Guid? LocationId { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid TaxingSchemeId { get; set; }
        public Guid? FakeId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? VehicleReceiveDate { get; set; }
        public SalesOrderPaymentStatus PaymentStatus { get; set; }
        public SalesOrderInventoryStatus InventoryStatus { get; set; }
        public decimal AmountPaid { get; set; }
        //public decimal Discount { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int Kilometrage { get; set; }
        public string VehicleReceiveFrom { get; set; }

        public List<FileAttachmentDto> Files { get; set; }
        public List<CreateSalesOrderProductLineDto> ProductLines { get; set; }
        public List<CreateSalesOrderServiceLineDto> ServiceLines { get; set; }

        public CreateSalesOrderDto()
        {
            Files = new List<FileAttachmentDto>();
            ProductLines = new List<CreateSalesOrderProductLineDto>();
            ServiceLines = new List<CreateSalesOrderServiceLineDto>();
        }
    }
}