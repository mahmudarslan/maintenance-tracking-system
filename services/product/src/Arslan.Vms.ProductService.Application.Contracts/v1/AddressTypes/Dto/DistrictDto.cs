using System;
using Volo.Abp.Application.Dtos;

namespace Arslan.Vms.ProductService.v1.AddressTypes.Dto
{
    public class DistrictDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CityId { get; set; }
    }
}