using Arslan.Vms.ProductService.EntityFrameworkCore;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.ProductService.Files
{
    public class FileAttachmentRepository : EfCoreRepository<IProductServiceDbContext, FileAttachment, Guid>, IFileAttachmentRepository
    {
        public FileAttachmentRepository(IDbContextProvider<IProductServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}