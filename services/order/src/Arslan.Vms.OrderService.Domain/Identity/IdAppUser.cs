using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Users;

namespace Arslan.Vms.OrderService.Identity
{
    public class AppUser : FullAuditedAggregateRoot<Guid>//, IUser
    {
        public virtual Guid? TenantId { get; private set; }

        public virtual string UserName { get; private set; }

        public virtual string Name { get; private set; }

        public virtual string Surname { get; private set; }

        public virtual string Email { get; private set; }

        public virtual bool EmailConfirmed { get; private set; }

        public virtual string PhoneNumber { get; private set; }

        public virtual bool PhoneNumberConfirmed { get; private set; }

        public virtual string WorkPhoneNumber { get; set; }
        public virtual string HomePhoneNumber { get; set; }
        public virtual string FaxNumber { get; set; }
        public virtual DateTime? BirthDate { get; set; }

        public virtual List<UserVehicle> UserVehicles { get; set; }
    }
}
