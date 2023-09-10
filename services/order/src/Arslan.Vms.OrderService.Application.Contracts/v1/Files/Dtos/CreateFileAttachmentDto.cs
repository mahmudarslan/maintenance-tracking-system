using System;

namespace Arslan.Vms.OrderService.v1.Files.Dtos
{
    public class CreateFileAttachmentDto
    {
        public Guid? TenantId { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public Guid DownloadGuid { get; set; }
        public bool UseDownloadUrl { get; set; }
        public string DownloadUrl { get; set; }
        public string Path { get; set; }
        public bool IsNew { get; set; }
    }
}