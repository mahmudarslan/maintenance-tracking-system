using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.PurchaseOrders
{
    public class PurchaseOrderReceiveLine : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid PurchaseOrderId { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual DateTime ReceiveDate { get; protected set; }
        public virtual string Description { get; set; }
        [StringLength(100)]
        public virtual string VendorLineCode { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal Quantity { get; set; }
        public virtual Guid LocationId { get; set; }
        public virtual string Sublocation { get; protected set; }
        public virtual string QuantityUom { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityDisplay { get; set; }
        public virtual Guid ProductId { get; protected set; }
        public virtual int LineNum { get; protected set; }

        //[NotMapped]
        //public virtual Product Product { get; set; }

        protected PurchaseOrderReceiveLine()
        {

        }

        public PurchaseOrderReceiveLine(Guid id,
            Guid? tenantId,
            Guid purchaseOrderId,
            int version,
            int lineNum,
            string description,
            decimal quantity,
            Guid locationId,
            Guid productId,
            DateTime receiveDate) : base(id)
        {
            TenantId = tenantId;
            PurchaseOrderId = purchaseOrderId;
            Version = version;
            ReceiveDate = receiveDate;
            Description = description;
            Quantity = quantity;
            LocationId = locationId;
            ProductId = productId;
            LineNum = lineNum;
        }

        public void SetLineNum(int lineNum)
        {
            LineNum = lineNum;
        }
    }
}