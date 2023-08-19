using Arslan.Vms.AdministrationService.Addresses;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Arslan.Vms.AdministrationService.Users
{
    public class UserManager : IUserManager, IDomainService
    {
        private readonly IRepository<User, Guid> _userRepository;
        private readonly IRepository<Role, Guid> _roleRepository;
        private readonly IRepository<Address, Guid> _addressRepository;
        //private readonly IRepository<Vehicle, Guid> _vehicleRepository;
        //private readonly IRepository<UserVehicle, Guid> _userVehicleRepository;

        public UserManager(IRepository<User, Guid> userRepository,
            IRepository<Role, Guid> roleRepository,
            IRepository<Address, Guid> addressRepository
            //IRepository<Vehicle, Guid> vehicleRepository
           //IRepository<UserVehicle, Guid> userVehicleRepository
           )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _addressRepository = addressRepository;
            //_vehicleRepository = vehicleRepository;
            //_userVehicleRepository = userVehicleRepository;
        }


        public virtual async Task<User> CreateAsync([NotNull] User user)
        {
            await _userRepository.EnsureCollectionLoadedAsync(user, u => u.Roles);

            var roles = await _roleRepository.GetListAsync();

            //foreach (var roleName in roleNames)
            //{
            //    var role = roles.FirstOrDefault(f => f.NormalizedName == roleName);
            //    if (role != null && !user.IsInRole(role.Id))
            //    {
            //        user.AddRole(role.Id);
            //    }
            //}

            //foreach (var address in addresses)
            //{
            //    user.AddAddress(address.Id);
            //}

            //foreach (var vehicle in vehicles)
            //{
            //    user.AddVehicle(vehicle.Id);
            //}

            await _userRepository.InsertAsync(user, true);
            //await _addressRepository.InsertManyAsync(addresses, true);
            //await _vehicleRepository.InsertManyAsync(vehicles, true);

            return user;
        }






        public virtual async Task<User> UpdateAsync([NotNull] User user)
        {
            var userRepository = await _userRepository.GetQueryableAsync();



            //await UpdateAddress(user, addresses);




            await _userRepository.UpdateAsync(user);

            return user;
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }

        public virtual async Task<User> GetAsync(Guid id)
        {
            return await _userRepository.GetAsync(id);
        }


        public virtual async Task<User> AddAddressAsync(User user, Address address)
        {
            user.AddAddress(address.Id);

            await _userRepository.UpdateAsync(user);
            await _addressRepository.InsertAsync(address);

            return user;
        }


        async Task<User> UpdateAddress(User user, List<Address> addresses)
        {
            var userRepository = await _userRepository.GetQueryableAsync();
            var addressRepository = await _addressRepository.GetQueryableAsync();

            var dbAddresses = (from u in userRepository.Where(w => w.Id == user.Id)
                               from ua in u.Addresses
                               from a in addressRepository.Where(w => w.Id == ua.AddressId)
                               select a).ToList();

            var removedAddressIds = dbAddresses.Select(s => s.Id).Except(addresses.Select(s => s.Id)).ToList();
            var insertedAddressIds = addresses.Select(s => s.Id).Except(dbAddresses.Select(s => s.Id)).ToList();
            var aIds = new List<Guid>();
            aIds.AddRange(removedAddressIds);
            aIds.AddRange(insertedAddressIds);
            var updatedAddressIds = dbAddresses.Select(s => s.Id).Except(aIds);


            foreach (var addressId in removedAddressIds)
            {
                var address = addresses.FirstOrDefault(f => f.Id == addressId);
                await _addressRepository.DeleteAsync(address);

                user.RemoveAddress(addressId);
            }

            foreach (var addressId in insertedAddressIds)
            {
                var address = addresses.FirstOrDefault(f => f.Id == addressId);
                await _addressRepository.InsertAsync(address);

                user.AddAddress(addressId);
            }

            foreach (var addressId in updatedAddressIds)
            {
                var address = addresses.FirstOrDefault(f => f.Id == addressId);
                await _addressRepository.UpdateAsync(address);
            }

            return user;
        }







    }
}
