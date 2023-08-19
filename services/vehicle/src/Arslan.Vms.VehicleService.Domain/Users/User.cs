using Arslan.Vms.VehicleService.Users.UserVehicles;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Users;

namespace Arslan.Vms.VehicleService.Users
{
    public class User : FullAuditedAggregateRoot<Guid>, IUser
    {
        public virtual Guid? TenantId { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Name { get; set; }
        public virtual string Surname { get; set; }
        public virtual string Email { get; set; }
        public virtual bool EmailConfirmed { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual string WorkPhoneNumber { get; set; }
        public virtual string HomePhoneNumber { get; set; }
        public virtual string FaxNumber { get; set; }
        public virtual DateTime? BirthDate { get; set; }
        public virtual decimal Discount { get; protected set; }
        public virtual string VendorPermitNumber { get; set; }
        public virtual string Remarks { get; set; }
        public virtual string DefaultCarrier { get; set; }
        public virtual string DefaultPaymentMethod { get; protected set; }
        public virtual string Website { get; set; }
        public virtual Guid? PaymentTermId { get; set; }
        public virtual Guid? CurrencyId { get; set; }
        public virtual Guid? PaymentMethodId { get; set; }
        public virtual Guid? DefaultPricingSchemeId { get; set; }
        public virtual Guid? DefaultPaymentTermsId { get; set; }
        public virtual Guid? TaxingSchemeId { get; set; }
        public virtual bool IsTaxInclusivePricing { get; set; }
        public virtual List<UserRole> Roles { get; set; }
        public virtual List<UserAddress> Addresses { get; set; }
        public virtual List<UserVehicle> Vehicles { get; set; }

        public bool IsActive { get; set; }


        //protected virtual List<UserAddress> _addresses { get; set; }
        //protected virtual List<UserVehicle> _vehicles { get; set; }
        //protected virtual List<UserRole> _roles { get; set; }

        protected User() { }

        public User(Guid id, Guid? tenantId, [NotNull] string userName, [NotNull] string name, [NotNull] string surName, [NotNull] string email) : base(id)
        {
            Check.NotNull(userName, nameof(userName));
            Check.NotNull(name, nameof(name));
            Check.NotNull(surName, nameof(surName));
            Check.NotNull(email, nameof(email));

            TenantId = tenantId;
            UserName = userName;
            Name = name;
            Surname = surName;
            Email = email;
            ConcurrencyStamp = Guid.NewGuid().ToString();
            Roles = new List<UserRole>();
            //Addresses = new List<UserAddress>();
            //Vehicles = new List<UserVehicle>();
        }


        #region Role
        public void AddRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            if (IsInRole(roleId))
            {
                return;
            }

            Roles.Add(new UserRole(TenantId, base.Id, roleId));
        }

        public virtual bool IsInRole(Guid roleId)
        {
            Check.NotNull(roleId, nameof(roleId));

            return Roles.Any(r => r.RoleId == roleId);
        }
        #endregion

        #region Address

        public void AddAddress(Guid addressId)
        {
            Check.NotNull(addressId, nameof(addressId));

            if (IsInAddress(addressId))
            {
                return;
            }

            Addresses.Add(new UserAddress(TenantId, Id, addressId));
        }

        public void RemoveAddress(Guid addressId)
        {
            Check.NotNull(addressId, nameof(addressId));

            Addresses.RemoveAll(r => r.AddressId == addressId);
        }

        public virtual bool IsInAddress(Guid addressId)
        {
            Check.NotNull(addressId, nameof(addressId));

            return Addresses.Any(r => r.AddressId == addressId);
        }
        #endregion


        #region Vehicle
        public void AddVehicle(Guid vehicleId)
        {
            Check.NotNull(vehicleId, nameof(vehicleId));

            if (IsInAddress(vehicleId))
            {
                return;
            }

            Vehicles.Add(new UserVehicle(TenantId, Id, vehicleId));
        }

        public void RemoveVehicle(Guid vehicleId)
        {

            var userVehicle = Vehicles.FirstOrDefault(f => f.VehicleId == vehicleId);
            userVehicle.IsDeleted = true;
        }

        public void UpdateVehicle(Guid vehicleId)
        {
            var userVehicle = Vehicles.FirstOrDefault(f => f.VehicleId == vehicleId);
            userVehicle.IsDeleted = false;
        }

        public virtual bool IsInVehicle(Guid vehicleId)
        {
            Check.NotNull(vehicleId, nameof(vehicleId));

            return Vehicles.Any(r => r.VehicleId == vehicleId);
        }
        #endregion
    }
}