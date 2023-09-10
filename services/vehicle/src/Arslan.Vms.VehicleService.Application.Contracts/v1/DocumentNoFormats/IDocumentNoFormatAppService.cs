using Arslan.Vms.VehicleService.v1.DocumentNoFormats.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.VehicleService.v1.DocumentNoFormats
{
    public interface IDocumentNoFormatAppService
    {
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
        Task<List<DocumentNoFormatDto>> UpdateAsync(List<DocumentNoFormatDto> input);
        Task<List<DocTypeDto>> GetDocTypesAsync();
        Task<List<DocumentNoFormatDto>> GetAllAsync();
    }
}