//using Arslan.Vms.Base.DocumentNoFormats;
//using Arslan.Vms.Base.Taxes;
using Arslan.Vms.OrderService.Accounts.Transactions;
using Arslan.Vms.OrderService.DocumentNoFormats;
using Arslan.Vms.OrderService.Taxes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.PurchaseOrders
{
    public class PurchaseOrder : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual int Version { get; protected set; }
        [StringLength(30)]
        public virtual string OrderNumber { get; protected set; }
        [StringLength(100)]
        public virtual string VendorOrderNumber { get; set; }
        public virtual DateTime OrderDate { get; protected set; }
        public virtual Guid VendorId { get; protected set; }
        [StringLength(200)]
        public virtual string ContactName { get; set; }
        [StringLength(50)]
        public virtual string ContactPhone { get; set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal OrderSubTotal { get; protected set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal OrderTax1 { get; protected set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal OrderTax2 { get; protected set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal OrderExtra { get; protected set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal OrderTotal { get; protected set; }
        public virtual Guid TaxingSchemeId { get; protected set; }
        [Column(TypeName = "decimal(10,5)")]
        public virtual decimal Tax1Rate { get; protected set; }
        [Column(TypeName = "decimal(10,5)")]
        public virtual decimal Tax2Rate { get; protected set; }
        public virtual bool CalculateTax2OnTax1 { get; protected set; }
        [StringLength(100)]
        public virtual string Tax1Name { get; protected set; }
        [StringLength(100)]
        public virtual string Tax2Name { get; protected set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal AmountPaid { get; protected set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal Balance { get; protected set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal AncillaryExpenses { get; protected set; }
        public virtual bool AncillaryIsPercent { get; protected set; }
        [Column(TypeName = "decimal(20,10)")]
        public virtual decimal ExchangeRate { get; protected set; }
        [Column(TypeName = "decimal(21,5)")]
        public virtual decimal Total { get; protected set; }
        public virtual Guid? LocationId { get; set; }
        public virtual Guid CurrencyId { get; protected set; }
        public virtual Guid? PaymentTermId { get; set; }
        public virtual string SummaryLinePermutation { get; set; }
        public virtual string ReceiveRemarks { get; set; }
        public virtual string OrderRemarks { get; set; }
        public virtual DateTime DueDate { get; set; }
        public virtual PurchaseOrderPaymentStatus PaymentStatus { get; set; }
        public virtual PurchaseOrderInventoryStatus InventoryStatus { get; set; }
        public virtual bool IsCompleted { get; protected set; }
        public virtual bool IsTaxInclusive { get; protected set; }


        [NotMapped]
        private bool _IsStockChanged { get; set; }

        public virtual List<PurchaseOrderLine> OrderLines { get; protected set; }
        public virtual List<PurchaseOrderReceiveLine> ReceiveLines { get; protected set; }
        public virtual PurchaseOrderPayment PurchaseOrderPayment { get; protected set; }
        public virtual ICollection<PurchaseOrderAttachment> Attachments { get; protected set; }

        protected PurchaseOrder()
        {
        }

        public PurchaseOrder(Guid id,
            Guid? tenantId,
            DateTime orderDate,
            Guid vendorId,
            PurchaseOrderPaymentStatus paymentStatus,
            PurchaseOrderInventoryStatus ınventoryStatus,
            Guid taxSchemeId,
            Guid currencyId,
            decimal amountPaid,
            DocNoFormatManager _docNoFormatManager) : base(id)
        {
            TenantId = tenantId;
            Version = 1;
            OrderNumber = _docNoFormatManager.GenerateNumber((int)OrderDocNoType.PurchaseOrder).Result;
            OrderDate = orderDate;
            VendorId = vendorId;
            PaymentStatus = paymentStatus;
            InventoryStatus = ınventoryStatus;
            TaxingSchemeId = taxSchemeId;
            CurrencyId = currencyId;
            AmountPaid = amountPaid;
            OrderLines = new List<PurchaseOrderLine>();
            ReceiveLines = new List<PurchaseOrderReceiveLine>();
        }

        public void AddOrderLine(Guid id, int lineNum, string description, decimal quantity,
    decimal unitPrice, decimal discount, bool discountIsPercent, Guid productId, TaxCode taxCode)
        {
            OrderLines.Add(new PurchaseOrderLine(id, TenantId, Id, productId, Version, lineNum, description, quantity, unitPrice, discount, discountIsPercent, taxCode));

            if (_IsStockChanged == false)
            {
                _IsStockChanged = quantity != 0;
            }
        }

        public void AddReceiveLine(Guid id, int lineNum, string description, decimal quantity, decimal discount, bool discountIsPercent,
               Guid locationId, Guid productId, DateTime pickDate, decimal unitPrice, TaxCode taxCode)
        {
            lineNum += 1000;
            ReceiveLines.Add(new PurchaseOrderReceiveLine(id, TenantId, Id, Version, lineNum, description, quantity, locationId, productId, pickDate));
            OrderLines.Add(new PurchaseOrderLine(id, TenantId, Id, productId, Version, lineNum, description, quantity, unitPrice, discount, discountIsPercent, taxCode));
        }

        public void SetReceiveLine(Guid pickLineId, Guid orderLineId, int lineNum, string description, decimal quantity, decimal discount, bool discountIsPercent,
               Guid locationId, decimal unitPrice)
        {
            var orderLine = OrderLines.FirstOrDefault(f => f.Id == orderLineId);

            orderLine.SetSubTotal(quantity, unitPrice, discount, discountIsPercent);
            orderLine.SetLineNum(lineNum);
            orderLine.Description = description;


            var pickLine = ReceiveLines.FirstOrDefault(f => f.Id == pickLineId);
            pickLine.SetLineNum(lineNum);
            pickLine.Description = description;
            pickLine.Quantity = quantity;
            pickLine.LocationId = locationId;
        }

        public void SetOrderLine(Guid orderLineId, int lineNum, string description, decimal quantity, decimal discount, bool discountIsPercent, decimal unitPrice)
        {
            var orderLine = OrderLines.FirstOrDefault(f => f.Id == orderLineId);

            if (_IsStockChanged == false)
            {
                _IsStockChanged = orderLine.Quantity != quantity;
            }

            orderLine.SetSubTotal(quantity, unitPrice, discount, discountIsPercent);
            orderLine.SetLineNum(lineNum);
            orderLine.Description = description;

            SetSubTotal();
        }

        public void RemoveOrderLine(Guid orderLineId)
        {
            var orderLine = OrderLines.FirstOrDefault(f => f.Id == orderLineId);

            OrderLines.Remove(orderLine);
        }

        public void RemoveReceiveLine(Guid pickLineId, Guid orderLineId)
        {
            var orderLine = OrderLines.FirstOrDefault(f => f.Id == orderLineId);
            OrderLines.Remove(orderLine);

            var pickLine = ReceiveLines.FirstOrDefault(f => f.Id == pickLineId);
            ReceiveLines.Remove(pickLine);
        }

        public void AddAttachment(Guid id, Guid fileAttachmentId)
        {
            Attachments.Add(new PurchaseOrderAttachment(id, TenantId, Id, fileAttachmentId));
        }

        public void SetCancel(bool value)
        {
            IsDeleted = value;
        }

        public void IncreaseVersion()
        {
            Version += 1;
        }

        public void ChangeVendor(Guid vendorId)
        {
            VendorId = vendorId;
        }

        public bool IsStockChanged()
        {
            return _IsStockChanged;
        }

        void SetSubTotal()
        {
            OrderSubTotal = OrderLines.Select(s => s.SubTotal).Sum();
        }

        void SetTotal(TaxingScheme tax)
        {
            SetSubTotal();
            Total = OrderSubTotal;

            Tax1Name = tax.Tax1Name;
            Tax2Name = tax.Tax2Name;
            CalculateTax2OnTax1 = tax.CalculateTax2OnTax1;
            IsTaxInclusive = true;

            var taxCode = tax.TaxCodes.FirstOrDefault(f => f.Id == tax.DefaultTaxCodeId);
            Tax1Rate = taxCode.Tax1Rate;
            Tax2Rate = taxCode.Tax2Rate;
            OrderTax1 = taxCode.Tax1Rate;
            OrderTax2 = taxCode.Tax2Rate;

            decimal taxValue = 0;

            foreach (var line in OrderLines)
            {
                if (tax.CalculateTax2OnTax1)
                {
                    taxValue += line.SubTotal * line.Tax1Rate / 100 + line.SubTotal * line.Tax2Rate / 100;
                }
                else
                {
                    taxValue += line.SubTotal * line.Tax1Rate / 100;
                }
            }

            Total -= taxValue;
        }

        public void SetBalance(decimal amount, TaxingScheme tax)
        {
            SetTotal(tax);
            AmountPaid = amount;
            Balance = Total - AmountPaid;
        }

        public void SetCurrency()
        {


        }

        [NotMapped]
        public TransactionId PurchaseOrderTransactionId
        {
            get
            {
                return new TransactionId((int)OrderTransactionType.PurchaseOrder, Id);
            }
        }
    }
}