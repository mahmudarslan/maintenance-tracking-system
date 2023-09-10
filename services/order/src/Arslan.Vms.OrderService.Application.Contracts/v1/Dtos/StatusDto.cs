using System;

namespace Arslan.Vms.OrderService.v1.Dtos
{
    public class StatusDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class WorkStatusDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}