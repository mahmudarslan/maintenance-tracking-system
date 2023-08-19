using System;
using Volo.Abp.Application.Dtos;

namespace Arslan.Vms.IdentityService.v1.AddressTypes.Dto
{
    public class CountryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}