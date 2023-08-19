using System;
using Volo.Abp.Application.Dtos;

namespace Arslan.Vms.IdentityService.v1.AddressTypes.Dto
{
    public class CityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CountryId { get; set; }
    }
}