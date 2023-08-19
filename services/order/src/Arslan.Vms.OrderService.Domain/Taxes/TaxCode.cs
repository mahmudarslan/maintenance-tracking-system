using System;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.Taxes
{
    public class TaxCode : Entity<Guid>, IMultiTenant, ISoftDelete, IModificationAuditedObject, IHasModificationTime
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid TaxingSchemeId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual decimal Tax1Rate { get; protected set; }
        public virtual decimal Tax2Rate { get; protected set; }
        public virtual bool IsDeleted { get; set; }
        public virtual Guid? LastModifierId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }

        protected TaxCode() { }

        public TaxCode(Guid id, Guid? tenantId, Guid taxingSchemeId, string name, decimal tax1Rate, decimal tax2Rate) : base(id)
        {
            TenantId = tenantId;
            TaxingSchemeId = taxingSchemeId;
            Name = name;
            Tax1Rate = tax1Rate;
            Tax2Rate = tax2Rate;
        }
    }
}