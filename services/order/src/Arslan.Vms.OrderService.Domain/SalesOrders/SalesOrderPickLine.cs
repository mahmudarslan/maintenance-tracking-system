using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.SalesOrders
{
    public class SalesOrderPickLine : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid SalesOrderId { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual int LineNum { get; protected set; }
        public virtual string Description { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal Quantity { get; set; }
        public virtual Guid LocationId { get; set; }
        [StringLength(100)]
        public string Sublocation { get; set; }
        [StringLength(20)]
        public virtual string QuantityUom { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityDisplay { get; set; }
        public virtual Guid ProductId { get; protected set; }
        public virtual DateTime PickDate { get; set; }

        //[NotMapped]
        //public virtual Product Product { get; set; }

        protected SalesOrderPickLine()
        {

        }

        public SalesOrderPickLine(Guid id, Guid? tenantId, Guid salesOrderId, int version, int lineNum,
            string description, decimal quantity, Guid locationId, Guid productId,
            DateTime pickDate) : base(id)
        {
            TenantId = tenantId;
            SalesOrderId = salesOrderId;
            Version = version;
            LineNum = lineNum;
            Description = description;
            Quantity = quantity;
            LocationId = locationId;
            ProductId = productId;
            PickDate = pickDate;
        }

        public void SetLineNum(int lineNum)
        {
            LineNum = lineNum;
        }

        public void SetVersion(int version)
        {
            Version = version;
        }
    }
}