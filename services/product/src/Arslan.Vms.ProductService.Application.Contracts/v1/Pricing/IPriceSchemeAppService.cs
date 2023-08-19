using Arslan.Vms.ProductService.v1.Pricing.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.ProductService.v1.Pricing
{
    public interface IPricingSchemeAppService
    {
        Task<PricingSchemeDto> CreateAsync(CreateUpdatePricingSchemeDto input);
        Task<PricingSchemeDto> UpdateAsync(Guid id, CreateUpdatePricingSchemeDto input);
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
        Task<List<PricingSchemeDto>> GetAllAsync();
    }
}