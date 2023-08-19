using Arslan.Vms.OrderService.v1.Files.Dtos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.v1.Files
{
    [ApiController]
    //[RemoteService(Name = "ArslanVmsCore")]
    [Area("Base")]
    [ControllerName("File")]
    [Route("rest/api/latest/vms/base/file")]
    //[ApiVersion("1.0")]
    public class FileController : AbpController
    {
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IFileAppService _fileAppService;
        private readonly ICurrentTenant _currentTenant;
        ServiceConfigurationContext _context;
        string _uploadPath;
        public string _uploadFullPath;


        public FileController(IFileAppService fileAppService,
               ICurrentTenant currentTenant,
            IWebHostEnvironment environment,
            ServiceConfigurationContext context)
        {
            _fileAppService = fileAppService;
            _hostingEnvironment = environment;
            _currentTenant = currentTenant;
            _context = context;

            var configuration = _context.Services.GetConfiguration();
            var uploadDirectoryName = configuration["UploadDirectory:Name"];

            _uploadPath = _currentTenant.Id == null ? "host" : _currentTenant.Id.ToString();

            var rootDirectory = Directory.GetParent(_hostingEnvironment.ContentRootPath).FullName;

            _uploadFullPath = Path.Combine(rootDirectory, uploadDirectoryName, _uploadPath);
        }

        [HttpPost]
        [Route("Upload")]
        public virtual async Task Upload(Guid id, [FromForm(Name = "file")] IFormFile inputFile)
        {
            if (!Directory.Exists(_uploadFullPath))
            {
                Directory.CreateDirectory(_uploadFullPath);
            }

            foreach (var item in Request.Form.Files)
            {
                var file = new CreateFileAttachmentDto
                {
                    FileName = item.FileName.Split('.').First(),
                    ContentType = item.ContentType,
                    Extension = item.FileName.Split('.').Last(),
                    Path = _uploadPath,
                    DownloadGuid = id,
                    DownloadUrl = "upload/"
                };

                var fileId = await _fileAppService.CreateAsync(file);

                using var fileStream = System.IO.File.Create(Path.Combine(_uploadFullPath, fileId.ToString()));
                item.CopyTo(fileStream);
            }
        }

        [HttpGet]
        [Route("Download")]
        public virtual async Task<IActionResult> Download(Guid id)
        {
            var file = await _fileAppService.GetAsync(id);

            if (file == null) return NoContent();

            var path = Path.Combine(_uploadFullPath, file.Id.ToString());

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/octet-stream", $"{file.FileName}.{file.Extension}");
        }
    }
}