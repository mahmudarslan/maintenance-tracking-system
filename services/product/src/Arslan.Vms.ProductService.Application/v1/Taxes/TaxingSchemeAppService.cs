using Arslan.Vms.ProductService.Localization;
using Arslan.Vms.ProductService.Permissions;
using Arslan.Vms.ProductService.Taxes;
using Arslan.Vms.ProductService.v1.Taxes.Dtos;
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

namespace Arslan.Vms.ProductService.v1.Taxes
{
    [Authorize(ProductServicePermissions.TaxingScheme.Default)]
    public class TaxingSchemeAppService : ProductServiceAppService, ITaxingSchemeAppService
    {
        private readonly IRepository<TaxingScheme, Guid> _taxSchemeRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IStringLocalizer<ProductServiceResource> _localizer;

        public TaxingSchemeAppService(IRepository<TaxingScheme, Guid> taxSchemeRepository,
         IGuidGenerator guidGenerator,
         IStringLocalizer<ProductServiceResource> localizer,
         ICurrentTenant currentTenant)
        {
            _taxSchemeRepository = taxSchemeRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _localizer = localizer;
        }

        [Authorize(ProductServicePermissions.TaxingScheme.Create)]
        public async Task<TaxingSchemeDto> CreateAsync(CreateTaxingSchemeDto input)
        {
            var taxScheme = new TaxingScheme(_guidGenerator.Create(), _currentTenant.Id,
                input.Name, input.Tax1Name, input.Tax2Name, input.CalculateTax2OnTax1);

            foreach (var item in input.TaxCodes)
            {
                taxScheme.AddTaxCode(_guidGenerator.Create(), item.Name, item.Tax1Rate, item.Tax2Rate);
            }

            if (taxScheme.DefaultTaxCodeId == null && taxScheme.TaxCodes.Count > 0)
            {
                taxScheme.DefaultTaxCodeId = taxScheme.TaxCodes[0].Id;
            }

            var result = await _taxSchemeRepository.InsertAsync(taxScheme);

            return ObjectMapper.Map<TaxingScheme, TaxingSchemeDto>(result);
        }

        [Authorize(ProductServicePermissions.TaxingScheme.Update)]
        public async Task<TaxingSchemeDto> UpdateAsync(Guid id, UpdateTaxingSchemeDto input)
        {
            var dbData = await _taxSchemeRepository.GetAsync(id);
            dbData.SetName(input.Name);
            dbData.SetTax1Name(input.Tax1Name);
            dbData.SetTax2Name(input.Tax2Name);
            dbData.DefaultTaxCodeId = input.DefaultTaxCodeId;
            dbData.CalculateTax2OnTax1 = input.CalculateTax2OnTax1;

            dbData.AddTaxCode(_guidGenerator.Create(), "Taxable", 0, 0);
            dbData.AddTaxCode(_guidGenerator.Create(), "Non-Taxable", 0, 0);

            await _taxSchemeRepository.UpdateAsync(dbData);

            return ObjectMapper.Map<TaxingScheme, TaxingSchemeDto>(dbData);
        }

        [Authorize(ProductServicePermissions.TaxingScheme.List)]
        public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            loadOptions.PrimaryKey = new[] { "Id" };

            var taxSchemeRepository = await _taxSchemeRepository.GetQueryableAsync();

            var ls = taxSchemeRepository.Select(s =>
                     new TaxingSchemeDto
                     {
                         Id = s.Id,
                         CalculateTax2OnTax1 = s.CalculateTax2OnTax1,
                         DefaultTaxCodeId = s.DefaultTaxCodeId,
                         Name = s.Name,
                         Tax1Name = s.Tax1Name,
                         Tax2Name = s.Tax2Name
                     });

            return await DataSourceLoader.LoadAsync(ls, loadOptions);
        }

        public async Task<List<TaxingSchemeDto>> GetAllAsync()
        {
            var taxSchemeRepository = await _taxSchemeRepository.GetQueryableAsync();

            var ls = taxSchemeRepository.Select(s =>
                     new TaxingSchemeDto
                     {
                         Id = s.Id,
                         CalculateTax2OnTax1 = s.CalculateTax2OnTax1,
                         DefaultTaxCodeId = s.DefaultTaxCodeId,
                         Name = s.Name,
                         Tax1Name = s.Tax1Name,
                         Tax2Name = s.Tax2Name
                     }).ToList();

            return await Task.FromResult(ls);
        }
    }
}