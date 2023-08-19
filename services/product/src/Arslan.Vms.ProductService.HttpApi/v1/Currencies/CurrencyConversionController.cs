﻿using Arslan.Vms.ProductService.v1.Currencies.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.ProductService.v1.Currencies
{
    [ApiController]
    //[RemoteService(Name = "ArslanVmsCore")]
    [Area("Base")]
    [ControllerName("Currency")]
    [Route("rest/api/latest/vms/base/currencyConversion")]
    //[ApiVersion("1.0")]
    public class CurrencyConversionController : ProductServiceController, ICurrencyConversionAppService
    {
        private readonly ICurrencyConversionAppService _currencyConversionAppService;

        public CurrencyConversionController(ICurrencyConversionAppService currencyConversionAppService)
        {
            _currencyConversionAppService = currencyConversionAppService;
        }

        [HttpPut]
        public Task<List<CurrencyConversionDto>> UpdateAsync(List<CurrencyConversionDto> input)
        {
            return _currencyConversionAppService.UpdateAsync(input);
        }

        [HttpGet]
        public Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            return _currencyConversionAppService.GetListAsync(loadOptions);
        }

        [HttpGet]
        [Route("GetAll")]
        public Task<List<CurrencyConversionDto>> GetAllAsync()
        {
            return _currencyConversionAppService.GetAllAsync();
        }
    }
}