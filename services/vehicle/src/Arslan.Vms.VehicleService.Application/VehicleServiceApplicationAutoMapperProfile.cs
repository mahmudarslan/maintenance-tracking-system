using Arslan.Vms.VehicleService.Files;
using Arslan.Vms.VehicleService.v1.Files.Dtos;
using Arslan.Vms.VehicleService.v1.Vehicles.Dtos;
using Arslan.Vms.VehicleService.v1.VehicleTypes.Dtos;
using Arslan.Vms.VehicleService.Vehicles;
using Arslan.Vms.VehicleService.Vehicles.VehicleTypes;
using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Arslan.Vms.VehicleService;

public class VehicleServiceApplicationAutoMapperProfile : Profile
{
    public VehicleServiceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        #region Vehicle
        CreateMap<Vehicle, VehicleDto>()
            .Ignore(i => i.BrandId)
            .Ignore(i => i.UserId);
        //.Ignore(i => i.UserVehicleId);
        #endregion

        #region VehicleType
        CreateMap<VehicleType, VehicleTypeDto>();
        #endregion 

        #region Attachment
        CreateMap<FileAttachment, FileAttachmentDto>();
        #endregion
    }
}
