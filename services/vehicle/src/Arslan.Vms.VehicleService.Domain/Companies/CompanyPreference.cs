using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.Companies
{
    public class CompanyPreference : FullAuditedEntity<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid? CurrencyId { get; protected set; }
        public virtual Guid? CompanyId { get; protected set; }
        public virtual Guid? CostingMethodId { get; protected set; }
        public virtual Guid? MeasureLenghtId { get; protected set; }
        public virtual Guid? MeasureWeightId { get; protected set; }

        protected CompanyPreference()
        {

        }

        public CompanyPreference(Guid id, Guid? tenantId, Guid? companyId, Guid? currencyId, Guid? costingMethodId, Guid? measureLenghtId, Guid? measureWeightId) : base(id)
        {
            TenantId = tenantId;
            CurrencyId = currencyId;
            CompanyId = companyId;
            CostingMethodId = costingMethodId;
            MeasureLenghtId = measureLenghtId;
            MeasureWeightId = measureWeightId;
        }
    }
}