using Arslan.Vms.InventoryService.DocumentNoFormats;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.InventoryService.StockAdjustments
{
    public class StockAdjustment : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual int Version { get; protected set; }
        [StringLength(30)]
        public virtual string AdjustmentNumber { get; protected set; }
        public virtual string Remarks { get; set; }
        public virtual bool IsCancelled { get; protected set; }

        public List<StockAdjustmentLine> Lines { get; protected set; }
        public virtual ICollection<StockAdjustmentAttachment> Attachments { get; protected set; }

        [NotMapped]
        private bool _IsStockChanged { get; set; }

        protected StockAdjustment()
        {
        }

        public StockAdjustment(Guid id, Guid? tenantId, DocNoFormatManager docNoFormatManager, string remarks) : base(id)
        {
            TenantId = tenantId;
            Version = 1;
            AdjustmentNumber = docNoFormatManager.GenerateNumber((int)InvDocNoType.StockAdjust).Result;
            Remarks = remarks;

            Lines = new List<StockAdjustmentLine>();
            Attachments = new List<StockAdjustmentAttachment>();
        }


        public List<StockAdjustmentLine> AddLine(Guid id, Guid productId, Guid locationId, decimal quantityBefore, decimal quantityAfter)
        {
            var line = new StockAdjustmentLine(id, TenantId, Id, productId, locationId, Version, quantityBefore, quantityAfter);
            Lines.Add(line);

            if (_IsStockChanged == false)
            {
                _IsStockChanged = quantityAfter != 0;
            }

            return Lines;
        }

        public void SetLine(Guid id, Guid productId, Guid locationId, decimal quantityBefore, decimal quantityAfter, bool? isDeleted)
        {
            var line = Lines.FirstOrDefault(f => f.Id == id);

            if (_IsStockChanged == false)
            {
                _IsStockChanged = line.QuantityAfter != quantityAfter; //|| line.IsDeleted == true;
            }

            //orderLine.SetSubTotal(quantity, unitPrice, Discount);
            //orderLine.SetLineNum(lineNum);
            //orderLine.Description = description;
            //line.IsDeleted = isDeleted ?? false;

            //SetTotalPrice();
        }

        public void SetCancel(bool cancel)
        {
            IsCancelled = cancel;
            IsDeleted = cancel;
        }

        public void IncreaseVersion()
        {
            Version += 1;
        }

        public bool IsStockChanged()
        {
            return _IsStockChanged;
        }
    }
}