using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.DocumentNoFormats
{
    public class DocNoFormat : BasicAggregateRoot<Guid>, IDocNoFormat, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual int DocNoType { get; protected set; }
        public virtual int NextNumber { get; protected set; }
        public virtual int MinDigits { get; protected set; } 
        public virtual string Prefix { get; protected set; } 
        public virtual string Suffix { get; protected set; }

        protected DocNoFormat(){ }

        public DocNoFormat(Guid id, int nextNumber, int minDigits, string prefix, string suffix, int docType, Guid? tenantId = null) : base(id)
        {
            NextNumber = nextNumber;
            MinDigits = minDigits;
            Prefix = prefix;
            Suffix = suffix;
            TenantId = tenantId;
            DocNoType = docType;
        }

        public void IncreaseNumber()
        {
            NextNumber += 1;
        }

        public void SetNextNumber(int nextNumber)
        {
            NextNumber = nextNumber;
        }

        public void SetPrefix(string prefix)
        {
            Prefix = prefix;
        }

        public void SetSuffix(string suffix)
        {
            Suffix = suffix;
        }

        public override string ToString()
        {
            return $"{Prefix}{NextNumber.ToString().PadLeft(MinDigits, '0')}{Suffix}";
        }

    }
}