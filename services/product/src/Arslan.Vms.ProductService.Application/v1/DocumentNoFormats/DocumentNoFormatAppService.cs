using Arslan.Vms.ProductService.DocumentNoFormats;
using Arslan.Vms.ProductService.Localization;
using Arslan.Vms.ProductService.Permissions;
using Arslan.Vms.ProductService.v1.DocumentNoFormats.Dtos;
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

namespace Arslan.Vms.ProductService.v1.DocumentNoFormats
{
    [Authorize(ProductServicePermissions.DocNumbers.Default)]
    public class DocumentNoFormatAppService : ProductServiceAppService, IDocumentNoFormatAppService
    {
        private readonly IDocNoFormatManager _documentNoFormatManger;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;
        private readonly IStringLocalizer<ProductServiceResource> _localizer;
        private readonly IRepository<DocNoFormat, Guid> _docNoFormatRepository;

        public DocumentNoFormatAppService(IDocNoFormatManager documentNoFormatManager,
         IRepository<DocNoFormat, Guid> docNoFormatRepository,
         IGuidGenerator guidGenerator,
         IStringLocalizer<ProductServiceResource> localizer,
         ICurrentTenant currentTenant)
        {
            _documentNoFormatManger = documentNoFormatManager;
            _docNoFormatRepository = docNoFormatRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
            _localizer = localizer;
        }

        [Authorize(ProductServicePermissions.DocNumbers.Update)]
        public async Task<List<DocumentNoFormatDto>> UpdateAsync(List<DocumentNoFormatDto> input)
        {
            foreach (var item in input)
            {
                await _documentNoFormatManger.Update(item.Id, item.Prefix, item.NextNumber, item.Suffix);
            }

            return input;
        }

        [Authorize(ProductServicePermissions.DocNumbers.List)]
        public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            loadOptions.PrimaryKey = new[] { "Id" };

            var docNoFormatRepository = await _docNoFormatRepository.GetQueryableAsync();

            var ls = docNoFormatRepository.Select(s =>
                      new DocumentNoFormatDto
                      {
                          DocNoType = s.DocNoType,
                          Id = s.Id,
                          MinDigits = s.MinDigits,
                          NextNumber = s.NextNumber,
                          Prefix = s.Prefix,
                          Suffix = s.Suffix
                      });

            return await DataSourceLoader.LoadAsync(ls, loadOptions);
        }

        public async Task<List<DocumentNoFormatDto>> GetAllAsync()
        {
            var docNoFormatRepository = await _docNoFormatRepository.GetQueryableAsync();

            var ls = docNoFormatRepository.Select(s =>
                      new DocumentNoFormatDto
                      {
                          DocNoType = s.DocNoType,
                          Id = s.Id,
                          MinDigits = s.MinDigits,
                          NextNumber = s.NextNumber,
                          Prefix = s.Prefix,
                          Suffix = s.Suffix
                      }).ToList();

            return await Task.FromResult(ls);
        }

        public async Task<List<DocTypeDto>> GetDocTypesAsync()
        {
            var data = new List<DocTypeDto>();

            foreach (var line in Enum.GetValues(typeof(DocNoType)))
            {
                data.Add(new DocTypeDto { Id = (int)line, Name = _localizer["DocType:" + line.ToString()].Value });
            }

            return await Task.FromResult(data);
        }
    }
}