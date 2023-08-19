using Arslan.Vms.ProductService.Currencies;
using Arslan.Vms.ProductService.Localization;
using Arslan.Vms.ProductService.Permissions;
using Arslan.Vms.ProductService.v1.Currencies.Dtos;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.ProductService.v1.Currencies
{
    [Authorize(ProductServicePermissions.CurrencyConversion.Default)]
    public class CurrencyConversionAppService : ProductServiceAppService, ICurrencyConversionAppService
    {
        private readonly IRepository<CurrencyConversion, Guid> _currencyConversionRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IStringLocalizer<ProductServiceResource> _localizer;

        public CurrencyConversionAppService(IRepository<CurrencyConversion, Guid> currencyConversionRepository,
         IGuidGenerator guidGenerator,
         IStringLocalizer<ProductServiceResource> localizer,
         ICurrentTenant currentTenant)
        {
            _currencyConversionRepository = currencyConversionRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _localizer = localizer;
        }

        [Authorize(ProductServicePermissions.CurrencyConversion.Update)]
        public async Task<List<CurrencyConversionDto>> UpdateAsync(List<CurrencyConversionDto> input)
        {
            foreach (var item in input)
            {
                await _currencyConversionRepository.UpdateAsync(new CurrencyConversion(_guidGenerator.Create(), item.CurrencyConversionId, item.CurrencyId, item.ExchangeRate));
            }

            return input;
        }

        [Authorize(ProductServicePermissions.CurrencyConversion.List)]
        public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            loadOptions.PrimaryKey = new[] { "Id" };

            var currencyConversionRepository = await _currencyConversionRepository.GetQueryableAsync();

            var ls = currencyConversionRepository.Select(s =>
                     new CurrencyConversionDto
                     {
                         Id = s.Id,
                         CurrencyConversionId = s.CurrencyConversionId,
                         CurrencyId = s.CurrencyId,
                         ExchangeRate = s.ExchangeRate
                     });

            return await DataSourceLoader.LoadAsync(ls, loadOptions);
        }

        public async Task<List<CurrencyConversionDto>> GetAllAsync()
        {
            var currencyConversionRepository = await _currencyConversionRepository.GetQueryableAsync();

            var ls = currencyConversionRepository.Select(s =>
                     new CurrencyConversionDto
                     {
                         Id = s.Id,
                         CurrencyConversionId = s.CurrencyConversionId,
                         CurrencyId = s.CurrencyId,
                         ExchangeRate = s.ExchangeRate
                     }).ToList();

            return await Task.FromResult(ls);
        }
    }
}