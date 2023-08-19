using System;
using Volo.Abp.Application.Dtos;

namespace Arslan.Vms.AdministrationService.v1.Vendors.Dtos
{
    public class VendorLineDto : AuditedEntityDto<Guid>
    {
        public Guid? VendorId { get; set; }
        public Guid? ProductId { get; set; }
        public string ProductCode { get; set; }
        public decimal Price { get; set; }
    }
}