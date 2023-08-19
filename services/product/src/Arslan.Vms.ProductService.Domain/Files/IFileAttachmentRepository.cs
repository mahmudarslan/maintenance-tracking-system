using System;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.ProductService.Files
{
    public interface IFileAttachmentRepository : IRepository<FileAttachment, Guid>
    {

    }
}