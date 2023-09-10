using System;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.IdentityService.Files
{
    public interface IFileAttachmentRepository : IRepository<FileAttachment, Guid>
    {

    }
}