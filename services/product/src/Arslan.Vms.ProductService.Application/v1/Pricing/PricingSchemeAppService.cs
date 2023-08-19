using Arslan.Vms.ProductService.Localization;
using Arslan.Vms.ProductService.Permissions;
using Arslan.Vms.ProductService.Pricing;
using Arslan.Vms.ProductService.v1.Pricing.Dtos;
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

namespace Arslan.Vms.ProductService.v1.Pricing
{
    [Authorize(ProductServicePermissions.PricingScheme.Default)]
    public class PricingSchemeAppService : ProductServiceAppService, IPricingSchemeAppService
    {
        private readonly IRepository<PricingScheme, Guid> _pricingSchemeRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IStringLocalizer<ProductServiceResource> _localizer;

        public PricingSchemeAppService(IRepository<PricingScheme, Guid> pricingSchemeRepository,
         IGuidGenerator guidGenerator,
         IStringLocalizer<ProductServiceResource> localizer,
         ICurrentTenant currentTenant)
        {
            _pricingSchemeRepository = pricingSchemeRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _localizer = localizer;
        }

        [Authorize(ProductServicePermissions.PricingScheme.Create)]
        public async Task<PricingSchemeDto> CreateAsync(CreateUpdatePricingSchemeDto input)
        {
            var result = await _pricingSchemeRepository.InsertAsync(new PricingScheme(_guidGenerator.Create(), _currentTenant.Id, input.Name, input.CurrencyId, input.IsTaxInclusive));

            return ObjectMapper.Map<PricingScheme, PricingSchemeDto>(result);
        }

        [Authorize(ProductServicePermissions.PricingScheme.Update)]
        public async Task<PricingSchemeDto> UpdateAsync(Guid id, CreateUpdatePricingSchemeDto input)
        {
            var dbData = await _pricingSchemeRepository.GetAsync(id);
            dbData.Set(input.Name, input.CurrencyId);
            dbData.IsTaxInclusive = input.IsTaxInclusive;
            await _pricingSchemeRepository.UpdateAsync(dbData);

            return ObjectMapper.Map<PricingScheme, PricingSchemeDto>(dbData);
        }

        [Authorize(ProductServicePermissions.PricingScheme.List)]
        public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            loadOptions.PrimaryKey = new[] { "Id" };
            var pricingSchemeRepository = await _pricingSchemeRepository.GetQueryableAsync();

            var ls = pricingSchemeRepository.Select(s =>
                     new PricingSchemeDto
                     {
                         Id = s.Id,
                         CurrencyId = s.CurrencyId,
                         IsTaxInclusive = s.IsTaxInclusive,
                         Name = s.Name
                     });

            return await DataSourceLoader.LoadAsync(ls, loadOptions);
        }

        public async Task<List<PricingSchemeDto>> GetAllAsync()
        {
            var pricingSchemeRepository = await _pricingSchemeRepository.GetQueryableAsync();

            var ls = pricingSchemeRepository.Select(s =>
                     new PricingSchemeDto
                     {
                         Id = s.Id,
                         CurrencyId = s.CurrencyId,
                         IsTaxInclusive = s.IsTaxInclusive,
                         Name = s.Name
                     }).ToList();

            return await Task.FromResult(ls);
        }
    }
}