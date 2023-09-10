using System;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.Products
{
    public class ProductAttachment : Entity, IMultiTenant
    {
        public virtual Guid? TenantId { get; protected set; }
        public virtual Guid ProductId { get; protected set; }
        public virtual Guid FileAttachmentId { get; protected set; }

        protected ProductAttachment() { }

        public ProductAttachment(Guid? tenantId, Guid productId, Guid fileAttachmentId)
        {
            TenantId = tenantId;
            ProductId = productId;
            FileAttachmentId = fileAttachmentId;
        }

        public override object[] GetKeys()
        {
            return new object[] { ProductId, FileAttachmentId };
        }
    }
}