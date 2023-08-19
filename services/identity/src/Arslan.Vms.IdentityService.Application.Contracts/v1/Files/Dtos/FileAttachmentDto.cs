using System;

namespace Arslan.Vms.IdentityService.v1.Files.Dtos
{
    public class FileAttachmentDto
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public string DownloadUrl { get; set; }
        public bool IsDeleted { get; set; }
    }
}