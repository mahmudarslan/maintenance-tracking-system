using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Pricing
{
    public class PricingScheme : BasicAggregateRoot<Guid>, IMultiTenant, ISoftDelete, IModificationAuditedObject, IHasModificationTime
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual string Name { get; protected set; }
        public virtual Guid CurrencyId { get; protected set; }
        public virtual bool IsTaxInclusive { get; set; }
        public virtual Guid? LastModifierId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }
        public virtual bool IsDeleted { get; set; }

        protected PricingScheme() { }

        public PricingScheme(Guid id, Guid? tenantId, string name, Guid currencyId, bool isTaxInclusive) : base(id)
        {
            TenantId = tenantId;
            Name = name;
            CurrencyId = currencyId;
            IsTaxInclusive = isTaxInclusive;
        }

        public void Set(string name, Guid currencyId)
        {
            Name = name;
            CurrencyId = currencyId;
        }
    }
}