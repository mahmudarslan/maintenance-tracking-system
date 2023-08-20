using Arslan.Vms.VehicleService.v1.DocumentNoFormats.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace Arslan.Vms.VehicleService.v1.DocumentNoFormats
{
    [ApiVersion("1.0")]
    [ApiController]
    [Area("Base")]
    [ControllerName("DocumentFormats")]
    [Route("vehicle/v{version:apiVersion}/documentNoFormats")]
    public class DocumentNoFormatController : VehicleServiceController, IDocumentNoFormatAppService
    {
        private readonly IDocumentNoFormatAppService _documentNoFormatAppService;

        public DocumentNoFormatController(IDocumentNoFormatAppService documentNoFormatAppService)
        {
            _documentNoFormatAppService = documentNoFormatAppService;
        }

        [HttpPut]
        public Task<List<DocumentNoFormatDto>> UpdateAsync(List<DocumentNoFormatDto> input)
        {
            return _documentNoFormatAppService.UpdateAsync(input);
        }

        [HttpGet]
        public Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            return _documentNoFormatAppService.GetListAsync(loadOptions);
        }

        [HttpGet]
        [Route("GetAll")]
        public Task<List<DocumentNoFormatDto>> GetAllAsync()
        {
            return _documentNoFormatAppService.GetAllAsync();
        }

        [HttpGet]
        [Route("DocTypes")]
        public Task<List<DocTypeDto>> GetDocTypesAsync()
        {
            return _documentNoFormatAppService.GetDocTypesAsync();
        }
    }
}