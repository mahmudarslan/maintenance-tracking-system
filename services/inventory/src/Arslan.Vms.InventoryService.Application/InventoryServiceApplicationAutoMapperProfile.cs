using Arslan.Vms.InventoryService.StockAdjustments;
using Arslan.Vms.InventoryService.v1.StockAdjustments.Dtos;
using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Arslan.Vms.InventoryService;

public class InventoryServiceApplicationAutoMapperProfile : Profile
{
    public InventoryServiceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        #region Stock Adjustment
        CreateMap<StockAdjustment, StockAdjustmentDto>()
           .Ignore(i => i.Status);

        CreateMap<StockAdjustmentLine, StockAdjustmentLineDto>();
        #endregion
    }
}
