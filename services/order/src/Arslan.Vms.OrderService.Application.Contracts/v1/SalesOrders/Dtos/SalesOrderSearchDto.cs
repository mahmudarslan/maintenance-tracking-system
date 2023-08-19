using Arslan.Vms.OrderService.SalseOrders;
using System;

namespace Arslan.Vms.OrderService.v1.SalesOrders.Dtos
{
    public class SalesOrderSearchDto
    {
        public Guid Id { get; set; }
        public Guid VehicleModelId { get; set; }
        public Guid VehicleBrandId { get; set; }
        public Guid? LocationId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? VehicleReceiveDate { get; set; }
        public SalesOrderPaymentStatus PaymentStatus { get; set; }
        public SalesOrderInventoryStatus InventoryStatus { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string PlateNo { get; set; }
        public string OrderNumber { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public string ModelName { get; set; }
        public decimal Total { get; set; }
        public decimal Balance { get; set; }
        public decimal AmountPaid { get; set; }
        public bool IsDeleted { get; set; }
    }
}