using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.IdentityService.Companies
{
    public class CompanyAttachment : Entity, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid CompanyId { get; protected set; }
        public virtual Guid FileAttachmentId { get; protected set; }

        protected CompanyAttachment() { }

        public CompanyAttachment(Guid? tenantId, Guid companyId, Guid fileAttachmentId)
        {
            TenantId = tenantId;
            CompanyId = companyId;
            FileAttachmentId = fileAttachmentId;
        }

        public override object[] GetKeys()
        {
            return new object[] { CompanyId, FileAttachmentId };
        }
    }
}