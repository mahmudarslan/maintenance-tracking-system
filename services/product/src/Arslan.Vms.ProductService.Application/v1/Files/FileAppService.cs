using Arslan.Vms.ProductService.Files;
using Arslan.Vms.ProductService.Permissions;
using Arslan.Vms.ProductService.v1.Files.Dtos;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.v1.Files
{
    [Authorize(ProductServicePermissions.UploadFiles.Default)]
    public class FileAppService : ProductServiceAppService, IFileAppService
    {
        private IRepository<FileAttachment, Guid> _fileAttachmentRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public FileAppService(IRepository<FileAttachment, Guid> fileAttachmentRepository,
                              IGuidGenerator guidGenerator,
                              ICurrentTenant currentTenant)
        {
            _fileAttachmentRepository = fileAttachmentRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }

        [Authorize(ProductServicePermissions.UploadFiles.Create)]
        public virtual async Task<Guid> CreateAsync(CreateFileAttachmentDto inputFile)
        {
            var file = new FileAttachment(
                _currentTenant.Id
                , inputFile.FileName
                , inputFile.Extension
                , inputFile.ContentType
                , inputFile.Data
                , inputFile.DownloadGuid
                , inputFile.UseDownloadUrl
                , null
                , inputFile.Path
                , inputFile.IsNew);

            await _fileAttachmentRepository.InsertAsync(file, true);

            return file.Id;
        }

        [Authorize(ProductServicePermissions.UploadFiles.List)]
        public virtual async Task<FileAttachmentDto> GetAsync(Guid id)
        {
            var result = await _fileAttachmentRepository.FirstOrDefaultAsync(f => f.Id == id);

            if (result == null)
            {
                return null;
            }

            return new FileAttachmentDto
            {
                DownloadUrl = result.DownloadUrl,
                FileName = result.FileName,
                Id = result.Id,
                Extension = result.Extension,
                Path = result.Path,
                IsDeleted = result.IsDeleted
            };
        }
    }
}