using Volo.Abp.Domain.Entities.Auditing;
using System;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Domain.Entities;

namespace Arslan.Vms.ProductService.Files
{
    public interface IFileAttachment : IAggregateRoot<Guid>, IMultiTenant
    {
        public string FileName { get; }
        public string Extension { get; }
        public string ContentType { get; }
        public byte[] Data { get; }
        public Guid DownloadGuid { get; }
        public bool UseDownloadUrl { get; }
        public string DownloadUrl { get; }
        public string Path { get; }
        public bool IsNew { get; }
    }
}