using Arslan.Vms.ProductService.v1.Pricing.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.ProductService.v1.Pricing
{
    [ApiVersion("1.0")]
    [ApiController]
    [Area("Base")]
    [ControllerName("PricingScheme")]
    [Route("product/v{version:apiVersion}/pricingScheme")]
    public class PricingSchemeController : ProductServiceController, IPricingSchemeAppService
    {
        private readonly IPricingSchemeAppService _priceSchemeAppService;

        public PricingSchemeController(IPricingSchemeAppService priceSchemeAppService)
        {
            _priceSchemeAppService = priceSchemeAppService;
        }

        [HttpPost]
        public Task<PricingSchemeDto> CreateAsync(CreateUpdatePricingSchemeDto input)
        {
            return _priceSchemeAppService.CreateAsync(input);
        }

        [HttpPut]
        public Task<PricingSchemeDto> UpdateAsync(Guid id, CreateUpdatePricingSchemeDto input)
        {
            return _priceSchemeAppService.UpdateAsync(id, input);
        }

        [HttpGet]
        public Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            return _priceSchemeAppService.GetListAsync(loadOptions);
        }

        [HttpGet]
        [Route("GetAll")]
        public Task<List<PricingSchemeDto>> GetAllAsync()
        {
            return _priceSchemeAppService.GetAllAsync();
        }
    }
}