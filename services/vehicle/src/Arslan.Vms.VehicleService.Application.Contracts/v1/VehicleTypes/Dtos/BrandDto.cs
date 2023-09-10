using System;
using Volo.Abp.Application.Dtos;

namespace Arslan.Vms.VehicleService.v1.VehicleTypes.Dtos
{
    public class BrandDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}