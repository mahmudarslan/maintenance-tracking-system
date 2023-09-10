using Volo.Abp.Domain.Entities.Auditing;
using System;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.IdentityService.Files
{
    public class FileAttachment : FullAuditedAggregateRoot<Guid>, IFileAttachment, IMultiTenant
    {
        public virtual Guid? TenantId { get; private set; }
        public virtual string FileName { get; private set; }
        public virtual string Extension { get; private set; }
        public virtual string ContentType { get; private set; }
        public virtual byte[] Data { get; private set; }
        public virtual Guid DownloadGuid { get; private set; }
        public virtual bool UseDownloadUrl { get; private set; }
        public virtual string DownloadUrl { get; private set; }
        public virtual string Path { get; private set; }
        public virtual bool IsNew { get; private set; }

        protected FileAttachment() { }

        public FileAttachment(Guid? tenantId, string fileName, string extension, string contentType, byte[] data, Guid downloadGuid, bool useDownloadUrl, string downloadUrl, string path, bool ısNew)
        {
            TenantId = tenantId;
            FileName = fileName;
            Extension = extension;
            ContentType = contentType;
            Data = data;
            DownloadGuid = downloadGuid;
            UseDownloadUrl = useDownloadUrl;
            DownloadUrl = downloadUrl;
            Path = path;
            IsNew = ısNew;
        }
    }
}