using System;
using Volo.Abp.Application.Dtos;

namespace Arslan.Vms.VehicleService.v1.VehicleTypes.Dtos
{
    public class ModelDto
    {
        public Guid Id { get; set; }
        public Guid? BrandId { get; set; }
        public Guid ModelTypeId { get; set; }
        public string Name { get; set; }
    }
}