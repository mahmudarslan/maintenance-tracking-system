using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.InventoryService.DocumentNoFormats
{
    public class InvDocNoFormat : BasicAggregateRoot<Guid>, IMultiTenant, IDocNoFormat
    {
        public virtual Guid? TenantId { get; set; }
        public virtual int DocNoType { get; set; }
        public virtual int NextNumber { get; set; }
        public virtual int MinDigits { get; set; }
        [StringLength(30)]
        public virtual string Prefix { get; set; }
        [StringLength(30)]
        public virtual string Suffix { get; set; }

        public InvDocNoFormat()
        {

        }

        public InvDocNoFormat(Guid id, int nextNumber, int minDigits, string prefix, string suffix, InvDocNoType docType, Guid? tenantId = null) : base(id)
        {
            NextNumber = nextNumber;
            MinDigits = minDigits;
            Prefix = prefix;
            Suffix = suffix;
            TenantId = tenantId;
            DocNoType = (int)docType;
        }
    }
}