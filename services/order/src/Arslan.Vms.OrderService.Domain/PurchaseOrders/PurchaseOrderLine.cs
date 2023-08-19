﻿using Arslan.Vms.OrderService.Taxes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.PurchaseOrders
{
    public class PurchaseOrderLine : Entity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual int Version { get; protected set; }
        public virtual Guid PurchaseOrderId { get; protected set; }
        public virtual Guid ProductId { get; protected set; }
        public virtual int LineNum { get; protected set; }
        public virtual string Description { get; set; }
        [StringLength(100)]
        public virtual string VendorLineCode { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal Quantity { get; protected set; }
        [Column(TypeName = "decimal(18,5)")]
        public virtual decimal UnitPrice { get; protected set; }
        public virtual string QuantityUom { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public virtual decimal QuantityDisplay { get; set; }
        [Column(TypeName = "decimal(18,5)")]
        public virtual decimal Discount { get; protected set; }
        public virtual bool DiscountIsPercent { get; protected set; }
        public virtual Guid TaxCodeId { get; set; }
        [Column(TypeName = "decimal(10,5)")]
        public virtual decimal Tax1Rate { get; set; }
        [Column(TypeName = "decimal(10,5)")]
        public virtual decimal Tax2Rate { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal SubTotal { get; protected set; }
        public virtual bool ServiceCompleted { get; protected set; }

        protected PurchaseOrderLine()
        {

        }

        public PurchaseOrderLine(
            Guid id,
            Guid? tenantId,
            Guid purchaseOrderId,
            Guid productId,
            int version,
            int lineNum,
            string description,
            decimal quantity,
            decimal unitPrice,
            decimal discount,
            bool discountIsPercent,
            TaxCode taxCode) : base(id)
        {
            TenantId = tenantId;
            PurchaseOrderId = purchaseOrderId;
            Version = version;
            Description = description;
            Quantity = quantity;
            UnitPrice = unitPrice;
            ProductId = productId;
            LineNum = lineNum;
            Discount = discount;
            DiscountIsPercent = discountIsPercent;
            TaxCodeId = taxCode.Id;
            Tax1Rate = taxCode.Tax1Rate;
            Tax2Rate = taxCode.Tax2Rate;

            SetSubTotal(quantity, unitPrice, discount, discountIsPercent);
        }

        public void SetSubTotal(decimal quantity, decimal unitPrice, decimal discount, bool discountIsPercent)
        {
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
            DiscountIsPercent = discountIsPercent;

            SubTotal = Quantity * unitPrice;

            if (DiscountIsPercent)
            {
                SubTotal -= SubTotal * Discount / 100;
            }
            else
            {
                SubTotal -= Discount;
            }
        }

        public void SetLineNum(int lineNum)
        {
            LineNum = lineNum;
        }
    }
}