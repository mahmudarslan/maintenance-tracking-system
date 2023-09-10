//using Arslan.Vms.Inventory;
//using Arslan.Vms.Payments;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Arslan.Vms.OrderService.SalseOrders;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.SalesOrders.Versions
{
    public class SalesOrderVersion : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid SalesOrderId { get; set; }


        public virtual Guid? TenantId { get; protected set; }
        public virtual int Version { get; protected set; }
        [StringLength(500)]
        public virtual string OrderNumber { get; protected set; }
        public virtual DateTime OrderDate { get; protected set; }
        public virtual Guid CustomerId { get; protected set; }
        public virtual Guid? ParentSalesOrderId { get; set; }
        public virtual Guid? LocationId { get; protected set; }
        public virtual Guid CurrencyId { get; set; }
        public virtual Guid? PaymentTermId { get; set; }
        public virtual DateTime? DueDate { get; set; }
        public virtual Guid? PricingSchemeId { get; set; }
        public virtual Guid TaxingSchemeId { get; set; }
        public virtual SalesOrderPaymentStatus PaymentStatus { get; protected set; }
        public virtual SalesOrderInventoryStatus InventoryStatus { get; protected set; }
        public virtual Guid? HeadTechnicianId { get; set; }
        public virtual Guid? VehicleId { get; set; }
        public virtual Guid? WorkorderTypeId { get; set; }
        public virtual DateTime? VehicleReceiveDate { get; set; }
        public virtual string Description { get; set; }
        public virtual string Notes { get; set; }
        public virtual int Kilometrage { get; set; }
        public virtual string VehicleReceiveFrom { get; set; }
        public virtual string OrderRemarks { get; set; }
        [StringLength(200)]
        public virtual string SalesRepresentative { get; set; }
        [StringLength(20)]
        public virtual string PONumber { get; set; }
        [StringLength(200)]
        public virtual string ContactName { get; set; }
        [StringLength(50)]
        public virtual string ContactPhone { get; set; }
        public virtual string ContactEmail { get; set; }
        public virtual DateTime? PickedDate { get; protected set; }
        public virtual DateTime? InvoicedDate { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal OrderSubTotal { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal OrderTax1 { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal OrderTax2 { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal OrderExtra { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal OrderTotal { get; set; }
        [Column(TypeName = "decimal(10,5)")]
        public virtual decimal Tax1Rate { get; set; }
        [Column(TypeName = "decimal(10,5)")]
        public virtual decimal Tax2Rate { get; set; }
        [StringLength(100)]
        public virtual string Tax1Name { get; set; }
        [StringLength(100)]
        public virtual string Tax2Name { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal? NonCustomerCost { get; set; }
        [Column(TypeName = "decimal(20,10)")]
        public virtual decimal ExchangeRate { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal Total { get; protected set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal AmountPaid { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal Balance { get; set; }
        public virtual string PickingRemarks { get; set; }
        public virtual string SummaryLinePermutation { get; set; }
        public virtual bool NonCustomerCostIsPercent { get; set; }
        public virtual bool CalculateTax2OnTax1 { get; set; }
        //public virtual bool IsCancelled { get; protected set; }
        public virtual bool IsQuote { get; protected set; }
        public virtual bool IsInvoiced { get; protected set; }
        public virtual bool IsCompleted { get; protected set; }
        public virtual bool IsTaxInclusive { get; protected set; }

        [NotMapped]
        public virtual ICollection<SalesOrderLineVersion> Lines { get; set; }
        [NotMapped]
        public virtual ICollection<SalesOrderPickLineVersion> PickLines { get; set; }
    }
}