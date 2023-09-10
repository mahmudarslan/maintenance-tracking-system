//using Arslan.Vms.Core.DocumentNoFormats;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.DocumentNoFormats
{
    public class OrderDocNoFormat : BasicAggregateRoot<Guid>, IMultiTenant, IDocNoFormat
    {
        public virtual Guid? TenantId { get; set; }
        public virtual int DocNoType { get; set; }
        public virtual int NextNumber { get; set; }
        public virtual int MinDigits { get; set; }
        [StringLength(30)]
        public virtual string Prefix { get; set; }
        [StringLength(30)]
        public virtual string Suffix { get; set; }

        public OrderDocNoFormat()
        {

        }

        public OrderDocNoFormat(Guid id, int nextNumber, int minDigits, string prefix, string suffix, OrderDocNoType docType, Guid? tenantId = null) : base(id)
        {
            NextNumber = nextNumber;
            MinDigits = minDigits;
            Prefix = prefix;
            Suffix = suffix;
            TenantId = tenantId;
            DocNoType = (int)docType;
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