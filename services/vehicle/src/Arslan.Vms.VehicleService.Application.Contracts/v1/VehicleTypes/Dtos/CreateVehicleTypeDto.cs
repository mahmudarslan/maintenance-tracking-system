using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arslan.Vms.VehicleService.v1.VehicleTypes.Dtos
{
    public class CreateVehicleTypeDto
    {
        public Guid? ParentId { get; set; }
        public string Name { get; set; }
    }
}