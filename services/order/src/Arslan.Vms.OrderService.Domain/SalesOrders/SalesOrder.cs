//using Arslan.Vms.Base.DocumentNoFormats;
//using Arslan.Vms.Base.Products;
//using Arslan.Vms.Base.Taxes;
using Arslan.Vms.OrderService.Accounts.Transactions;
using Arslan.Vms.OrderService.DocumentNoFormats;
using Arslan.Vms.OrderService.Inventory;
using Arslan.Vms.OrderService.SalseOrders;
using Arslan.Vms.OrderService.Taxes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.SalesOrders
{
    public class SalesOrder : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
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
        private bool _IsStockChanged { get; set; }

        #region Relation   
        public virtual List<SalesOrderLine> OrderLines { get; protected set; }
        public virtual List<SalesOrderPickLine> PickLines { get; protected set; }
        public virtual List<SalesOrderAttachment> Attachments { get; protected set; }
        #endregion


        protected SalesOrder()
        {
            OrderLines = new List<SalesOrderLine>();
            PickLines = new List<SalesOrderPickLine>();
            Attachments = new List<SalesOrderAttachment>();
        }

        public SalesOrder(Guid id,
            Guid? tenantId,
            DateTime orderDate,
            Guid customerId,
            Guid? locationId,
            SalesOrderPaymentStatus paymentStatus,
            SalesOrderInventoryStatus ınventoryStatus,
            Guid? headTechnicianId,
            Guid? userVehicleId,
            Guid? workorderTypeId,
            DateTime? vehicleReceiveDate,
            string description,
            string notes,
            int kilometrage,
            string vehicleReceiveFrom,
            DocNoFormatManager _docNoFormatManager,
            Guid taxSchemeId,
            Guid currencyId) : base(id)
        {
            TenantId = tenantId;
            Version = 1;
            OrderNumber = _docNoFormatManager.GenerateNumber((int)OrderDocNoType.SalesOrder).Result;
            OrderDate = orderDate;
            CustomerId = customerId;
            LocationId = locationId;
            PaymentStatus = paymentStatus;
            InventoryStatus = ınventoryStatus;
            HeadTechnicianId = headTechnicianId;
            VehicleId = userVehicleId;
            WorkorderTypeId = workorderTypeId;
            VehicleReceiveDate = vehicleReceiveDate;
            Description = description;
            Notes = notes;
            Kilometrage = kilometrage;
            VehicleReceiveFrom = vehicleReceiveFrom;
            TaxingSchemeId = taxSchemeId;
            CurrencyId = currencyId;

            OrderLines = new List<SalesOrderLine>();
            PickLines = new List<SalesOrderPickLine>();
            Attachments = new List<SalesOrderAttachment>();
        }

        public void AddOrderLine(Guid id, OrderProduct product, int lineNum, string description, decimal quantity,
            decimal unitPrice, decimal discount, bool discountIsPercent, Guid productId, Guid? technicianId, TaxCode taxCode)
        {
            //if (product.ProductType != ProductType.Service)
            //{
            //    throw new UserFriendlyException("Can't add Product to Service Line");
            //}

            if (lineNum == 0 || OrderLines.Any(a => a.LineNum == lineNum))
            {
                throw new UserFriendlyException("Line num dont 0 or same");
            }

            OrderLines.Add(new SalesOrderLine(id, TenantId, Id, productId, technicianId, Version, lineNum, description, quantity, unitPrice, discount, discountIsPercent, taxCode));

            if (_IsStockChanged == false)
            {
                _IsStockChanged = quantity != 0;
            }

            SetSubTotal();
        }

        public void AddPickLine(Guid id, OrderProduct product, int lineNum, string description,
            decimal quantity, decimal discount, bool discountIsPercent,
               Guid locationId, Guid productId, DateTime pickDate, decimal unitPrice, TaxCode taxCode)
        {
            //if (product.ProductType == ProductType.Service)
            //{
            //    throw new UserFriendlyException("Can't add Service to Product Line");
            //}

            if (lineNum == 0)
            {
                throw new UserFriendlyException("Line num dont 0");
            }

            lineNum += 1000;

            if (PickLines.Any(a => a.LineNum == lineNum))
            {
                throw new UserFriendlyException("Line num dont same");
            }

            PickLines.Add(new SalesOrderPickLine(id, TenantId, Id, Version, lineNum, description, quantity, locationId, productId, pickDate));
            OrderLines.Add(new SalesOrderLine(id, TenantId, Id, productId, null, Version, lineNum, description, quantity, unitPrice, discount, discountIsPercent, taxCode));
        }

        public void SetPickLine(Guid pickLineId, Guid orderLineId, int lineNum, string description, decimal quantity,
            decimal discount, bool discountIsPercent,
               Guid locationId, decimal unitPrice, TaxCode taxCode)
        {
            var orderLine = OrderLines.FirstOrDefault(f => f.Id == orderLineId);
            var pickLine = PickLines.FirstOrDefault(f => f.Id == pickLineId);

            //if (isDeleted == true)
            //{
            //    OrderLines.Remove(orderLine);
            //    PickLines.Remove(pickLine);

            //    return;
            //}

            if (lineNum == 0)
            {
                throw new UserFriendlyException("Line num dont 0");
            }

            if (lineNum < 1000)
            {
                lineNum += 1000;
            }

            orderLine.SetSubTotal(quantity, unitPrice, discount, discountIsPercent);
            orderLine.SetLineNum(lineNum);
            orderLine.Description = description;
            orderLine.TaxCodeId = taxCode.Id;
            orderLine.Tax1Rate = taxCode.Tax1Rate;
            orderLine.Tax2Rate = taxCode.Tax2Rate;

            pickLine.SetLineNum(lineNum);
            pickLine.Description = description;
            pickLine.Quantity = quantity;
            pickLine.LocationId = locationId;
        }

        public void SetOrderLine(Guid orderLineId, int lineNum, string description, decimal quantity,
            decimal discount, bool discountIsPercent, decimal unitPrice, TaxCode taxCode)
        {
            var orderLine = OrderLines.FirstOrDefault(f => f.Id == orderLineId);

            if (_IsStockChanged == false)
            {
                _IsStockChanged = orderLine.Quantity != quantity;
            }

            orderLine.SetSubTotal(quantity, unitPrice, discount, discountIsPercent);
            orderLine.SetLineNum(lineNum);
            orderLine.Description = description;
            orderLine.TaxCodeId = taxCode.Id;
            orderLine.Tax1Rate = taxCode.Tax1Rate;
            orderLine.Tax2Rate = taxCode.Tax2Rate;

            SetSubTotal();
        }

        public void RemoveOrderLine(SalesOrderLine orderLine)
        {
            OrderLines.Remove(orderLine);
        }

        public void RemovePickLine(SalesOrderPickLine pickLine)
        {
            var orderLine = OrderLines.FirstOrDefault(f => f.LineNum == pickLine.LineNum);

            OrderLines.Remove(orderLine);
            PickLines.Remove(pickLine);
        }

        public void AddAttachment(Guid id, Guid fileAttachmentId)
        {
            Attachments.Add(new SalesOrderAttachment(id, TenantId, Id, fileAttachmentId));
        }

        public void SetCancel(bool value)
        {
            IsDeleted = value;
            //IsCancelled = value;
        }

        public void IncreaseVersion()
        {
            Version += 1;

            foreach (var item in PickLines)
            {
                item.SetVersion(Version);
            }

            foreach (var item in OrderLines)
            {
                item.SetVersion(Version);
            }
        }

        public void ChangeCustomer(Guid customerId)
        {
            CustomerId = customerId;
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

        public void ChangeInventoryStatus(SalesOrderInventoryStatus status)
        {
            InventoryStatus = status;

            switch (status)
            {
                case SalesOrderInventoryStatus.Quote:
                    IsCompleted = false;
                    break;
                case SalesOrderInventoryStatus.Unfulfilled:
                    IsCompleted = false;
                    break;
                case SalesOrderInventoryStatus.Started:
                    IsCompleted = false;
                    break;
                case SalesOrderInventoryStatus.Fulfilled:
                    IsCompleted = true;
                    break;
                default:
                    IsCompleted = false;
                    break;
            }
        }

        public void ChangePaymentStatus(SalesOrderPaymentStatus status)
        {
            PaymentStatus = status;
        }

        public void ChangeOrderDate(DateTime date)
        {
            OrderDate = date;
        }

        private SalesOrderPaymentStatus GetPaymentStatus()
        {
            if (IsQuote)
            {
                return SalesOrderPaymentStatus.Quote;
            }
            if (Balance < decimal.Zero)
            {
                return SalesOrderPaymentStatus.Owing;
            }
            if (Balance == decimal.Zero && AmountPaid == decimal.Zero)
            {
                if (!IsInvoiced && InvoicedDate == null && !IsCompleted)
                {
                    return SalesOrderPaymentStatus.Uninvoiced;
                }
                return SalesOrderPaymentStatus.Paid;
            }
            if (Balance == decimal.Zero && AmountPaid != decimal.Zero)
            {
                return SalesOrderPaymentStatus.Paid;
            }
            if (Balance > decimal.Zero && AmountPaid > decimal.Zero)
            {
                return SalesOrderPaymentStatus.Partial;
            }
            if (!IsInvoiced)
            {
                return SalesOrderPaymentStatus.Uninvoiced;
            }
            return SalesOrderPaymentStatus.Invoiced;
        }

        //public InventoryAvailabilityStatus GetInventoryStatus(SalesOrderSummaryLineList list, SalesOrderSummaryLine line)
        //{

        //}

        [NotMapped]
        public TransactionId SalesOrderInvoiceTransactionId
        {
            get
            {
                return new TransactionId((int)OrderTransactionType.SalesOrderInvoice, Id);
            }
        }

        [NotMapped]
        public TransactionId SalesOrderServiceDoneTransactionId
        {
            get
            {
                return new TransactionId((int)OrderTransactionType.SalesOrderServiceDone, Id);
            }
        }

        [NotMapped]
        public TransactionId SalesOrderTransactionId
        {
            get
            {
                return new TransactionId((int)OrderTransactionType.SalesOrder, Id);
            }
        }

        public TransactionId SalesOrderFulfillmentTransactionId(Guid? childId)
        {
            return new TransactionId((int)OrderTransactionType.SalesOrderFulfillment, Id, childId);
        }



    }
}