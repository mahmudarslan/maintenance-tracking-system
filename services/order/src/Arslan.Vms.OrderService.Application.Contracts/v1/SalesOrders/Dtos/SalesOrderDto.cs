using Arslan.Vms.OrderService.SalseOrders;
using Arslan.Vms.OrderService.v1.Files.Dtos;
using System;
using System.Collections.Generic;

namespace Arslan.Vms.OrderService.v1.SalesOrders.Dtos
{
    public class SalesOrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? HeadTechnicianId { get; set; }
        public Guid? VehicleId { get; set; }
        public Guid? WorkorderTypeId { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid TaxingSchemeId { get; set; }
        public Guid? LocationId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? VehicleReceiveDate { get; set; }
        public SalesOrderPaymentStatus PaymentStatus { get; set; }
        public SalesOrderInventoryStatus InventoryStatus { get; set; }
        public int Version { get; set; }
        public string OrderNumber { get; set; }
        public string OrderRemarks { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public int Kilometrage { get; set; }
        public string VehicleReceiveFrom { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
        public bool IsDeleted { get; set; }

        public List<SalesOrderProductLineDto> ProductLines { get; set; }
        public List<SalesOrderServiceLineDto> ServiceLines { get; set; }
        public List<FileAttachmentDto> Files { get; set; }

        public SalesOrderDto()
        {
            ProductLines = new List<SalesOrderProductLineDto>();
            ServiceLines = new List<SalesOrderServiceLineDto>();
            Files = new List<FileAttachmentDto>();
        }
    }
}