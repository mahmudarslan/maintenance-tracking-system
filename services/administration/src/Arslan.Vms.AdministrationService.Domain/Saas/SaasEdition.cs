using JetBrains.Annotations;
using System;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace Arslan.Vms.AdministrationService.Saas;

public class SaasEdition : FullAuditedAggregateRoot<Guid>
{
    public virtual string Name { get; protected set; }
    public virtual string NormalizedName { get; protected set; }
    public virtual bool IsStatic { get; set; }

    protected SaasEdition() { }

    public SaasEdition(Guid id, [NotNull] string name, bool isStatic = false)
    {
        Id = id;
        SetName(name);
        IsStatic = isStatic;
    }

    protected internal virtual void SetName([NotNull] string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name), SaasEditionMaxNameLengthConsts.MaxNameLength);
        NormalizedName = Name.ToUpperInvariant();
    }
}