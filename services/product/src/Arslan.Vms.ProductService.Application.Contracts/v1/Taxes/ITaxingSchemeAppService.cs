using Arslan.Vms.ProductService.v1.Taxes.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.ProductService.v1.Taxes
{
    public interface ITaxingSchemeAppService
    {
        Task<TaxingSchemeDto> CreateAsync(CreateTaxingSchemeDto input);
        Task<TaxingSchemeDto> UpdateAsync(Guid id, UpdateTaxingSchemeDto input);
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
        Task<List<TaxingSchemeDto>> GetAllAsync();
    }
}