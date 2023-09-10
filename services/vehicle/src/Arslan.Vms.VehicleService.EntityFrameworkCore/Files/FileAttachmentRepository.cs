using Arslan.Vms.VehicleService.EntityFrameworkCore;
using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.VehicleService.Files
{
    public class FileAttachmentRepository : EfCoreRepository<IVehicleServiceDbContext, FileAttachment, Guid>, IFileAttachmentRepository
    {
        public FileAttachmentRepository(IDbContextProvider<IVehicleServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }
    }
}