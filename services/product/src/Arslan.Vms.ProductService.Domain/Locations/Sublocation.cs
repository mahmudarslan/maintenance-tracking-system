using System;
using Volo.Abp.Domain.Entities;

namespace Arslan.Vms.ProductService.Locations
{
    public class Sublocation : Entity<Guid>
    {
        public Guid? LocationId { get; set; }
        public string Name { get; set; }
    }
}