using Arslan.Vms.ProductService.v1.Taxes.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.ProductService.v1.Taxes
{
    [ApiVersion("1.0")]
    [ApiController]
    [Area("Base")]
    [ControllerName("TaxingScheme")]
    [Route("product/v{version:apiVersion}/taxingScheme")]
    public class TaxingSchemeController : ProductServiceController, ITaxingSchemeAppService
    {
        private readonly ITaxingSchemeAppService _taxSchemeAppService;

        public TaxingSchemeController(ITaxingSchemeAppService taxSchemeAppService)
        {
            _taxSchemeAppService = taxSchemeAppService;
        }

        [HttpPost]
        public Task<TaxingSchemeDto> CreateAsync(CreateTaxingSchemeDto input)
        {
            return _taxSchemeAppService.CreateAsync(input);
        }

        [HttpPut]
        public Task<TaxingSchemeDto> UpdateAsync(Guid id, UpdateTaxingSchemeDto input)
        {
            return _taxSchemeAppService.UpdateAsync(id, input);
        }

        [HttpGet]
        public Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            return _taxSchemeAppService.GetListAsync(loadOptions);
        }

        [HttpGet]
        [Route("GetAll")]
        public Task<List<TaxingSchemeDto>> GetAllAsync()
        {
            return _taxSchemeAppService.GetAllAsync();
        }
    }
}