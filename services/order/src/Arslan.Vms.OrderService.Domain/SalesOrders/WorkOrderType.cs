using System;
using Volo.Abp.Domain.Entities;

namespace Arslan.Vms.OrderService.SalesOrders
{
    public class WorkOrderType : BasicAggregateRoot<Guid>
    {
        public virtual string Name { get; set; }


        public WorkOrderType()
        {

        }

        public WorkOrderType(string name)
        {
            Name = name;
        }

        public WorkOrderType(Guid id, string name) : base(id)
        {
            Name = name;
        }
    }
}