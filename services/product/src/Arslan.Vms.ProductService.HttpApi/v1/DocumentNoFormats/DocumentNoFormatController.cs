using Arslan.Vms.ProductService.v1.DocumentNoFormats.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.ProductService.v1.DocumentNoFormats
{
    [ApiController]
    //[RemoteService(Name = "ArslanVmsCore")]
    [Area("Base")]
    [ControllerName("DocumentFormats")]
    [Route("rest/api/latest/vms/base/documentNoFormats")]
    //[ApiVersion("1.0")]
    public class DocumentNoFormatController : ProductServiceController, IDocumentNoFormatAppService
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