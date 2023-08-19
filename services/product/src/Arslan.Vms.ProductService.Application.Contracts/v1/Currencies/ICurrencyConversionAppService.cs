using Arslan.Vms.ProductService.v1.Currencies.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.ProductService.v1.Currencies
{
    public interface ICurrencyConversionAppService
    {
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
        Task<List<CurrencyConversionDto>> UpdateAsync(List<CurrencyConversionDto> input);
        Task<List<CurrencyConversionDto>> GetAllAsync();
    }
}