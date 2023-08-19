using System;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.VehicleService.Files
{
    public interface IFileAttachmentRepository : IRepository<FileAttachment, Guid>
    {

    }
}