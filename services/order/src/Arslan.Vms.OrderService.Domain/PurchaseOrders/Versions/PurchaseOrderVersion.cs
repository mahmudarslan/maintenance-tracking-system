using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.PurchaseOrders.Versions
{
    public class PurchaseOrderVersion : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid PurchaseOrderId { get; set; }


        public virtual Guid? TenantId { get; set; }
        public virtual int Version { get; set; }
        [StringLength(30)]
        public virtual string OrderNumber { get; set; }
        [StringLength(100)]
        public virtual string VendorOrderNumber { get; set; }
        public virtual DateTime OrderDate { get; set; }
        public virtual Guid VendorId { get; set; }
        public virtual Guid? PaymentTermId { get; set; }
        [StringLength(100)]
        public virtual string Carrier { get; set; }
        [StringLength(200)]
        public virtual string ContactName { get; set; }
        [StringLength(50)]
        public virtual string ContactPhone { get; set; }
        public virtual Guid? VendorAddressId { get; set; }
        //public virtual Guid? ShipToAddressId { get; set; }
        //public virtual DateTime RequestShipDate { get; set; }
        public virtual Guid TaxingSchemeId { get; set; }
        public virtual string OrderRemarks { get; set; }
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
        public virtual bool CalculateTax2OnTax1 { get; set; }
        [StringLength(100)]
        public virtual string Tax1Name { get; set; }
        [StringLength(100)]
        public virtual string Tax2Name { get; set; }
        public virtual string ReceiveRemarks { get; set; }
        public virtual DateTime DueDate { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal AmountPaid { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal Balance { get; set; }
        //public virtual string ReturnRemarks { get; set; }
        //[Column(TypeName = "decimal(21,5)")]
        //public virtual decimal ReturnSubTotal { get; set; }
        //[Column(TypeName = "decimal(21,5)")]
        //public virtual decimal ReturnTax1 { get; set; }
        //[Column(TypeName = "decimal(21,5)")]
        //public virtual decimal ReturnTax2 { get; set; }
        //[Column(TypeName = "decimal(21,5)")]
        //public virtual decimal ReturnExtra { get; set; }
        //[Column(TypeName = "decimal(21,5)")]
        //public virtual decimal ReturnTotal { get; set; }
        //[Column(TypeName = "decimal(21,5)")]
        //public virtual decimal ReturnFee { get; set; }
        public virtual string UnstockRemarks { get; set; }
        //public virtual bool Tax1OnShipping { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal AncillaryExpenses { get; set; }
        public virtual bool AncillaryIsPercent { get; set; }
        public virtual Guid? LocationId { get; set; }
        //public virtual bool ShowShipping { get; set; }
        //[StringLength(255)]
        //public virtual string ShipToCompanyName { get; set; }
        public virtual Guid CurrencyId { get; set; }
        [Column(TypeName = "decimal(20,10)")]
        public virtual decimal ExchangeRate { get; set; }
        //public virtual bool Tax2OnShipping { get; set; }
        public virtual string SummaryLinePermutation { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal Total { get; set; }
        public virtual int PaymentStatus { get; set; }
        public virtual int InventoryStatus { get; set; }
        public virtual bool IsCancelled { get; set; }
        public virtual bool IsCompleted { get; set; }
        public virtual bool IsTaxInclusive { get; set; }
        //public virtual Guid? CarrierId { get; set; }
        //public virtual Guid? PaymentId { get; set; }

        [NotMapped]
        public virtual List<PurchaseOrderLineVersion> OrderLines { get; set; }
        [NotMapped]
        public virtual List<PurchaseOrderReceiveLineVersion> ReceiveLines { get; set; }
    }
}