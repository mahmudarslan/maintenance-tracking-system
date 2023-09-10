using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.Taxes
{
    public class TaxingScheme : AuditedAggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual string Tax1Name { get; protected set; }
        public virtual string Tax2Name { get; protected set; }
        public virtual bool CalculateTax2OnTax1 { get; set; }
        public virtual Guid? DefaultTaxCodeId { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual List<TaxCode> TaxCodes { get; set; }

        protected TaxingScheme() { }

        public TaxingScheme(Guid id, Guid? tenantId, string name, string tax1Name,
            string tax2Name, bool calculateTax2OnTax1) : base(id)
        {
            TenantId = tenantId;
            Name = name;
            Tax1Name = tax1Name;
            Tax2Name = tax2Name;
            CalculateTax2OnTax1 = calculateTax2OnTax1;
            TaxCodes = new List<TaxCode>();
        }

        public void AddTaxCode(Guid id, string name, decimal tax1Rate, decimal tax2Rate)
        {
            TaxCodes.Add(new TaxCode(id, TenantId, Id, name, tax1Rate, tax2Rate));
        }

        public void SetDefaultTaxCode(Guid id)
        {
            DefaultTaxCodeId = id;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetTax1Name(string name)
        {
            Tax1Name = name;
        }

        public void SetTax2Name(string name)
        {
            Tax2Name = name;
        }


    }
}