using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.InventoryService.DocumentNoFormats
{
    public interface IDocNoFormat : IAggregateRoot<Guid>, IMultiTenant
    {
        public int DocNoType { get; }
        public int NextNumber { get; }
        public int MinDigits { get; }
        public string Prefix { get; }
        public string Suffix { get; }
    }
}