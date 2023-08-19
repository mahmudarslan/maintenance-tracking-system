using System;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.AdministrationService.Files
{
    public interface IFileAttachmentRepository : IRepository<FileAttachment, Guid>
    {

    }
}