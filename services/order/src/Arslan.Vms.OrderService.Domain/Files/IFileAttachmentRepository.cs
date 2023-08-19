using System;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.OrderService.Files
{
    public interface IFileAttachmentRepository : IRepository<OrderAttachment, Guid>
    {

    }
}