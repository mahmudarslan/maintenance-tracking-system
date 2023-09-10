using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arslan.Vms.VehicleService.v1.VehicleTypes.Dtos
{
    public class VehicleTypeDto
    {
        public Guid? ParentId { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool IsDeleted { get; set; }
    }
}