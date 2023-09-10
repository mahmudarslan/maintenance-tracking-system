using Arslan.Vms.IdentityService.Addresses;
using Arslan.Vms.IdentityService.Permissions;
using Arslan.Vms.IdentityService.Roles;
using Arslan.Vms.IdentityService.Users;
using Arslan.Vms.IdentityService.v1.Addresses.Dtos;
using Arslan.Vms.IdentityService.v1.Customers;
using Arslan.Vms.IdentityService.v1.Customers.Dtos;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;

namespace Arslan.Vms.IdentityService.v1.Customers
{
    [Authorize(IdentityServicePermissions.Customer.Default)]
    public class CustomerAppService : IdentityServiceAppService//, ICustomerAppService
    {

        #region Fields
        private IUserRepository _appUserRepository;
        private IUserManager _userManagerRepository;
        //IUserRoleRepository _userRoleRepository;
        IRoleRepository _roleRepository;
        //private IVehicleTypeRepository _vehicleTypeRepository;
        private IDataFilter _dataFilter;
        //private readonly IRepository<Vehicle, Guid> _vehicleRepository;
        private readonly IRepository<Address, Guid> _addressRepository;
        #endregion

        #region Ctor
        public CustomerAppService(
                IUserRepository appUserRepository,
                IRoleRepository roleRepository,
                //IUserRoleRepository userRoleRepository,
                //IVehicleTypeRepository vehicleTypeRepository,
                IUserManager userManagerRepository,
                //IRepository<Vehicle, Guid> vehicleRepository,
                IRepository<Address, Guid> addressRepository,
                IDataFilter dataFilter
                )
        {
            _appUserRepository = appUserRepository;
            _userManagerRepository = userManagerRepository;
            _roleRepository = roleRepository;
            //_userRoleRepository = userRoleRepository;
            _dataFilter = dataFilter;
            //_vehicleRepository = vehicleRepository;
            //_vehicleTypeRepository = vehicleTypeRepository;
            _addressRepository = addressRepository;
        }
        #endregion

        [Authorize(IdentityServicePermissions.Customer.Create)]
        public async Task<CustomerDto> CreateAsync(CreateCustomerDto input)
        {
            var user = new User(GuidGenerator.Create(), CurrentTenant.Id, input.UserName, input.Name, input.Surname, input.Email)
            {
                HomePhoneNumber = input.HomePhoneNumber,
                FaxNumber = input.FaxNumber,
                BirthDate = input.BirthDate,
                WorkPhoneNumber = input.WorkPhoneNumber
            };
            var roleNames = new List<string> { RoleConsts.Customer };
            var addresses = new List<Address>();
            //var vehicles = new List<Vehicle>();

            //Address
            foreach (var inputAddress in input.Addresses ?? Enumerable.Empty<CreateAddressDto>())
            {
                var insertedAddress = new Address(GuidGenerator.Create(), CurrentTenant.Id, inputAddress.AddressType, 1,
                    inputAddress.CountryId, inputAddress.CityId, inputAddress.DistrictId,
                        inputAddress.AddressName, inputAddress.Address1, inputAddress.Address2,
                        inputAddress.PostalCode, inputAddress.Remarks)
                {
                    CountryName = inputAddress.CountryName,
                    CityName = inputAddress.CityName,
                    DistrictName = inputAddress.DistrictName,
                    IsDefaultAddress = inputAddress.IsDefaultAddress,
                    IsBillingAddress = inputAddress.IsBillingAddress,
                    IsShippingAddress = inputAddress.IsShippingAddress
                };
                addresses.Add(insertedAddress);
            }

            //Vehicle
            //foreach (var inputVehicle in input.Vehicles ?? Enumerable.Empty<CreateVehicleDto>())
            //{
            //    vehicles.Add(new Vehicle(GuidGenerator.Create(), CurrentTenant.Id, inputVehicle.Plate, inputVehicle.Color, inputVehicle.Motor, inputVehicle.Chassis, inputVehicle.ModelId));
            //}

            await _userManagerRepository.CreateAsync(user);

            var result = ObjectMapper.Map<User, CustomerDto>(user);
            result.Addresses = ObjectMapper.Map<List<Address>, List<AddressDto>>(addresses);
            //result.Vehicles = ObjectMapper.Map<List<Vehicle>, List<VehicleDto>>(vehicles);

            return result;
        }

        [Authorize(IdentityServicePermissions.Customer.Update)]
        public async Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto input)
        {
            var identityUser = (await _appUserRepository.WithDetailsAsync()).FirstOrDefault(f => f.Id == id);
            identityUser.HomePhoneNumber = input.HomePhoneNumber;
            identityUser.FaxNumber = input.FaxNumber;
            identityUser.BirthDate = input.BirthDate;
            identityUser.WorkPhoneNumber = input.WorkPhoneNumber;

            var addresses = new List<Address>();
            //var vehicles = new List<Vehicle>();

            foreach (var inputAddress in input.Addresses ?? Enumerable.Empty<UpdateAddressDto>())
            {
                if (inputAddress.Id == Guid.Empty)
                {
                    var insertedAddress = new Address(GuidGenerator.Create(), CurrentTenant.Id, inputAddress.AddressType, 1,
                          inputAddress.CountryId, inputAddress.CityId, inputAddress.DistrictId,
                          inputAddress.AddressName, inputAddress.Address1, inputAddress.Address2,
                          inputAddress.PostalCode, inputAddress.Remarks)
                    {
                        CountryName = inputAddress.CountryName,
                        CityName = inputAddress.CityName,
                        DistrictName = inputAddress.DistrictName,
                        IsDefaultAddress = inputAddress.IsDefaultAddress,
                        IsBillingAddress = inputAddress.IsBillingAddress,
                        IsShippingAddress = inputAddress.IsShippingAddress
                    };

                    addresses.Add(insertedAddress);
                }
                else
                {
                    var updateUserAddress = (await _appUserRepository.GetAddresses(id)).FirstOrDefault(f => f.Id == inputAddress.Id);
                    updateUserAddress.Address1 = inputAddress.Address1;
                    updateUserAddress.Address2 = inputAddress.Address2;
                    updateUserAddress.AddressName = inputAddress.AddressName;
                    updateUserAddress.AddressType = inputAddress.AddressType;
                    updateUserAddress.CityId = inputAddress.CityId;
                    updateUserAddress.CountryId = inputAddress.CountryId;
                    updateUserAddress.DistrictId = inputAddress.DistrictId;
                    updateUserAddress.CityName = inputAddress.CityName;
                    updateUserAddress.CountryName = inputAddress.CountryName;
                    updateUserAddress.DistrictName = inputAddress.DistrictName;
                    updateUserAddress.IsBillingAddress = inputAddress.IsBillingAddress;
                    updateUserAddress.IsDefaultAddress = inputAddress.IsDefaultAddress;
                    updateUserAddress.IsShippingAddress = inputAddress.IsShippingAddress;
                    updateUserAddress.PostalCode = inputAddress.PostalCode;
                    updateUserAddress.Remarks = inputAddress.Remarks;

                    addresses.Add(updateUserAddress);
                }
            }

            //Vehicle
            //foreach (var inputVehicle in input.Vehicles ?? Enumerable.Empty<UpdateVehicleDto>())
            //{
            //    if (inputVehicle.Id == Guid.Empty && inputVehicle.IsDeleted == false)
            //    {
            //        //Create
            //        var vehicle = new Vehicle(GuidGenerator.Create(), CurrentTenant.Id, inputVehicle.Plate,
            //          inputVehicle.Color,
            //          inputVehicle.Motor,
            //          inputVehicle.Chassis,
            //          inputVehicle.ModelId);
            //        await _vehicleRepository.InsertAsync(vehicle, true);

            //        identityUser.AddVehicle(vehicle.Id);
            //    }
            //    else
            //    {

            //        if (inputVehicle.IsDeleted == false)
            //        {
            //            //Update
            //            var updateUserVehicle = (await _appUserRepository.GetVehicles(id)).FirstOrDefault(f => f.Id == inputVehicle.Id);
            //            updateUserVehicle.SetChassis(inputVehicle.Chassis);
            //            updateUserVehicle.SetColor(inputVehicle.Color);
            //            updateUserVehicle.SetModelId(inputVehicle.ModelId);
            //            updateUserVehicle.SetMotor(inputVehicle.Motor);
            //            updateUserVehicle.SetPlate(inputVehicle.Plate);
            //        }

            //        else
            //        {
            //            //delete
            //            identityUser.RemoveVehicle(inputVehicle.Id);
            //        }
            //    }
            //}

            await _appUserRepository.UpdateAsync(identityUser, true);

            var identityUserDto = new IdentityUserUpdateDto
            {
                Name = input.Name,
                UserName = input.UserName,
                Email = input.Email,
                Surname = input.Surname,
                PhoneNumber = input.PhoneNumber,
                ConcurrencyStamp = identityUser.ConcurrencyStamp
            };
            //await _identityUserAppService.UpdateAsync(id, identityUserDto);


            var result = ObjectMapper.Map<User, CustomerDto>(identityUser);
            result.Addresses = ObjectMapper.Map<List<Address>, List<AddressDto>>(addresses);
            //result.Vehicles = ObjectMapper.Map<List<Vehicle>, List<VehicleDto>>(vehicles);

            return result;
        }

        [Authorize(IdentityServicePermissions.Customer.Delete)]
        public async Task DeleteAsync(string key)
        {
            var user = await _appUserRepository.GetAsync(Guid.Parse(key));
            await _appUserRepository.DeleteAsync(user, true);
        }

        [Authorize(IdentityServicePermissions.Customer.Undo)]
        public async Task UndoAsync(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var user = await _appUserRepository.FirstOrDefaultAsync(f => f.Id == id);
                user.IsDeleted = false;
                await _appUserRepository.UpdateAsync(user, true);
            }
        }

        //[Authorize(IdentityServicePermissions.Customer.Undo)]
        //public async Task UndoVehcileAsync(Guid userId, Guid vehicleId)
        //{
        //    using (_dataFilter.Disable<ISoftDelete>())
        //    {
        //        await _appUserRepository.UndoVehicleAsync(userId, vehicleId);
        //    }
        //}

        public async Task<CustomerDto> GetAsync(Guid id, bool isDeleted)
        {
            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var appUserRepository = await _appUserRepository.GetQueryableAsync();
                //var userRoleRepository = await _userRoleRepository.GetQueryableAsync();
                var roleRepository = await _roleRepository.GetQueryableAsync();
                //var vehicleRepository = await _vehicleRepository.GetQueryableAsync();
                //var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();

                var appUser = (from user in appUserRepository
                               from userRole in user.Roles
                               from role in roleRepository.Where(w => w.Id == userRole.RoleId && 
                               w.NormalizedName == RoleConsts.Customer)
                               where user.Id == id
                               select new CustomerDto
                               {
                                   Id = user.Id,
                                   UserName = user.UserName,
                                   Name = user.Name,
                                   Surname = user.Surname,
                                   BirthDate = user.BirthDate,
                                   PhoneNumber = user.PhoneNumber,
                                   HomePhoneNumber = user.HomePhoneNumber,
                                   WorkPhoneNumber = user.WorkPhoneNumber,
                                   FaxNumber = user.FaxNumber,
                                   CreationTime = user.CreationTime,
                                   Email = user.Email,
                                   IsDeleted = user.IsDeleted
                               }).FirstOrDefault();


                //var vehicles = (from user in appUserRepository
                //                from uv in user.Vehicles
                //                from v in vehicleRepository.Where(w => w.Id == uv.VehicleId)
                //                from vehicleModel in vehicleTypeRepository.Where(w => w.Id == v.ModelId)
                //                select new VehicleDto
                //                {
                //                    Id = v.Id,
                //                    Plate = v.Plate,
                //                    Color = v.Color,
                //                    Motor = v.Motor,
                //                    Chassis = v.Chassis,
                //                    ModelId = v.ModelId,
                //                    BrandId = vehicleModel.ParentId.Value,
                //                    CreationTime = v.CreationTime
                //                }).ToList();

                var addresses = await _appUserRepository.GetAddresses(id);

                appUser.Addresses = ObjectMapper.Map<List<Address>, List<AddressDto>>(addresses);
                //appUser.Vehicles = vehicles;

                return await Task.FromResult(appUser);
            }
        }

        [Authorize(IdentityServicePermissions.Customer.List)]
        public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                loadOptions.PrimaryKey = new[] { "Id" };

                var appUserRepository = await _appUserRepository.GetQueryableAsync();
                //var userRoleRepository = await _userRoleRepository.GetQueryableAsync();
                var roleRepository = await _roleRepository.GetQueryableAsync();

                var ls = from user in appUserRepository
                          from userRole in user.Roles.Where(w => w.UserId == user.Id)
                          from role in roleRepository.Where(w => w.Id == userRole.RoleId && w.NormalizedName == RoleConsts.Customer)
                          select new CustomerListDto
                          {
                              Id = user.Id,
                              UserName = user.UserName,
                              Name = user.Name,
                              Surname = user.Surname,
                              BirthDate = user.BirthDate,
                              PhoneNumber = user.PhoneNumber,
                              HomePhoneNumber = user.HomePhoneNumber,
                              WorkPhoneNumber = user.WorkPhoneNumber,
                              FaxNumber = user.FaxNumber,
                              Email = user.Email,
                              CreationTime = user.CreationTime,
                              IsDeleted = user.IsDeleted,
                          };

                return await DataSourceLoader.LoadAsync(ls, loadOptions);
            }
        }

        public async Task<List<CustomerListDto>> GetAllAsync()
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var appUserRepository = await _appUserRepository.GetQueryableAsync();
                //var userRoleRepository = await _userRoleRepository.GetQueryableAsync();
                var roleRepository = await _roleRepository.GetQueryableAsync();

                var data = (from user in appUserRepository
                            from userRole in user.Roles.Where(w => w.UserId == user.Id)
                            from role in roleRepository.Where(w => w.Id == userRole.RoleId && w.NormalizedName == RoleConsts.Customer)
                            select new CustomerListDto
                            {
                                Id = user.Id,
                                Name = user.Name,
                                Surname = user.Surname,
                                BirthDate = user.BirthDate,
                                PhoneNumber = user.PhoneNumber,
                                Email = user.Email,
                                CreationTime = user.CreationTime,
                                UserName = user.UserName,
                                IsDeleted = user.IsDeleted,
                                FaxNumber = user.FaxNumber,
                                HomePhoneNumber = user.HomePhoneNumber,
                                WorkPhoneNumber = user.WorkPhoneNumber,
                            }).ToList();

                return await Task.FromResult(data);
            }
        }

        public async Task<List<CustomerSearchDto>> GetBySearchContent(string nameSurnameGsmPlateNo)
        {
            var appUserRepository = await _appUserRepository.GetQueryableAsync();
            //var userRoleRepository = await _userRoleRepository.GetQueryableAsync();
            var roleRepository = await _roleRepository.GetQueryableAsync();

            var result = (from user in appUserRepository
                          from userRole in user.Roles.Where(w => w.UserId == user.Id)
                          from role in roleRepository.Where(w => w.Id == userRole.RoleId && w.NormalizedName == RoleConsts.Customer)
                          where 
                          (user.Name + " " + user.Surname).Contains(nameSurnameGsmPlateNo) ||
                          user.PhoneNumber.Contains(nameSurnameGsmPlateNo)

                          select new CustomerSearchDto
                          {
                              Id = user.Id,
                              Name = user.Name,
                              Surname = user.Surname,
                              PhoneNumber = user.PhoneNumber,
                              SelectionField = $"{user.Name} {user.Surname} {user.PhoneNumber}",
                          }
                            ).ToList();

            return await Task.FromResult(result);
        }

        //public async Task<CustomerVehicleSearchDto> GetByVehicleId(Guid customerId, Guid vehicleId)
        //{
        //    using (_dataFilter.Disable<ISoftDelete>())
        //    {
        //        var appUserRepository = await _appUserRepository.GetQueryableAsync();
        //        //var userRoleRepository = await _userRoleRepository.GetQueryableAsync();
        //        //var vehicleRepository = await _vehicleRepository.GetQueryableAsync();
        //        var roleRepository = await _roleRepository.GetQueryableAsync();

        //        var result = (from user in appUserRepository
        //                      from userRole in user.Roles.Where(w => w.UserId == user.Id)
        //                      from role in roleRepository.Where(w => w.Id == userRole.RoleId && w.NormalizedName == RoleConsts.Customer)
        //                      //from uv in user.Vehicles
        //                      //from v in vehicleRepository.Where(w => w.Id == uv.VehicleId)

        //                      //where uv.UserId == customerId && uv.VehicleId == vehicleId

        //                      select new CustomerVehicleSearchDto
        //                      {
        //                          Id = user.Id,
        //                          VehicleId = uv.VehicleId,
        //                          Name = user.Name,
        //                          Surname = user.Surname,
        //                          PhoneNumber = user.PhoneNumber,
        //                          PlateNo = v.Plate,
        //                          SelectionField = $"{user.Name} {user.Surname} {user.PhoneNumber}  {v.Plate}"
        //                      }).FirstOrDefault();

        //        return await Task.FromResult(result);
        //    }
        //}

        //public async Task<List<CustomerVehiclehDto>> GetCustomerVehicles(Guid customerId)
        //{
        //    var appUserRepository = await _appUserRepository.GetQueryableAsync();
        //    var vehicleRepository = await _vehicleRepository.GetQueryableAsync();
        //    var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();

        //    var result = (from user in appUserRepository
        //                  from uv in user.Vehicles
        //                  from v in vehicleRepository.Where(w => w.Id == uv.VehicleId)
        //                  from model in vehicleTypeRepository.Where(w => w.Id == v.ModelId)
        //                  from brand in vehicleTypeRepository.Where(w => w.Id == model.ParentId)
        //                  where user.Id == customerId
        //                  select new CustomerVehiclehDto
        //                  {
        //                      Id = user.Id,
        //                      VehicleId = uv.VehicleId,
        //                      Name = $"{brand.Name} - {model.Name} - {v.Plate}",
        //                  }).ToList();

        //    return await Task.FromResult(result);
        //}

        //public async Task<CustomerVehiclehDto> GetCustomerVehicle(Guid userId, Guid vehicleId)
        //{
        //    using (_dataFilter.Disable<ISoftDelete>())
        //    {
        //        var appUserRepository = await _appUserRepository.GetQueryableAsync();
        //        var vehicleRepository = await _vehicleRepository.GetQueryableAsync();
        //        var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();

        //        var result = (from user in appUserRepository
        //                      from uv in user.Vehicles
        //                      from v in vehicleRepository.Where(w => w.Id == uv.VehicleId)
        //                      from model in vehicleTypeRepository.Where(w => w.Id == v.ModelId)
        //                      from brand in vehicleTypeRepository.Where(w => w.Id == model.ParentId)
        //                      where uv.UserId == userId && uv.VehicleId == vehicleId
        //                      select new CustomerVehiclehDto
        //                      {
        //                          Id = user.Id,
        //                          VehicleId = uv.VehicleId,
        //                          Name = $"{brand.Name} - {model.Name} - {v.Plate}",
        //                      }).FirstOrDefault();

        //        return await Task.FromResult(result);
        //    }
        //}

        public async Task<CustomerReportDto> GetCustomerDetails(Guid userId, Guid vehicleId)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var appUserRepository = await _appUserRepository.GetQueryableAsync();
                //var vehicleRepository = await _vehicleRepository.GetQueryableAsync();
                //var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();
                var addressRepository = await _addressRepository.GetQueryableAsync();

                var result = (from user in appUserRepository
                              //from uv in user.Vehicles
                              //from v in vehicleRepository.Where(w => w.Id == uv.VehicleId)
                              from userAddress in user.Addresses
                              from a in addressRepository.Where(w => w.Id == userAddress.AddressId)
                              //from model in vehicleTypeRepository.Where(w => w.Id == v.ModelId).DefaultIfEmpty()
                              //from modelType in vehicleTypeRepository.Where(w => w.Id == model.ParentId).DefaultIfEmpty()
                              //where uv.UserId == userId && uv.VehicleId == vehicleId
                              select new CustomerReportDto
                              {
                                  UserCn = user.Name + " " + user.Surname,
                                  HomePhoneNumber = user.HomePhoneNumber,
                                  PhoneNumber = user.PhoneNumber,
                                  WorkPhoneNumber = user.WorkPhoneNumber,
                                  Address = a.Address1,
                                  //VehicleChassis = v.Chassis,
                                  //VehicleColor = v.Color,
                                  //VehicleMotor = v.Motor,
                                  //VehiclePlateNo = v.Plate,
                                  //VehicleBrandName = modelType.Name,
                              }).FirstOrDefault();

                return await Task.FromResult(result);
            }
        }

        //public async Task<LoadResult> GetVehicleListAsync(DataSourceLoadOptions loadOptions)
        //{
        //    loadOptions.PrimaryKey = new[] { "Id" };

        //    using (_dataFilter.Disable<ISoftDelete>())
        //    {
        //        var appUserRepository = await _appUserRepository.GetQueryableAsync();
        //        //var vehicleRepository = await _vehicleRepository.GetQueryableAsync();
        //        //var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();

        //        var ls = 
        //                  from u in appUserRepository
        //                  from uv in u.Vehicles.Where(w => w.UserId == u.Id)
        //                  from v in vehicleRepository.Where(w => w.Id == uv.VehicleId)
        //                  from vtModel in vehicleTypeRepository.Where(w => w.Id == v.ModelId)
        //                  from vtBrand in vehicleTypeRepository.Where(w => w.Id == vtModel.ParentId)
        //                  select new VehicleListDto
        //                  {
        //                      UserId = u.Id,
        //                      UserCN = u.Name + " " + u.Surname,
        //                      BrandId = vtModel.ParentId.Value,
        //                      ModelId = v.ModelId,
        //                      Chassis = v.Chassis,
        //                      Color = v.Color,
        //                      Id = v.Id,
        //                      BrandName = vtBrand.Name,
        //                      ModelName = vtModel.Name,
        //                      Motor = v.Motor,
        //                      Plate = v.Plate,
        //                      CreationTime = v.CreationTime,
        //                      IsDeleted = v.IsDeleted,
        //                  };

        //        return await DataSourceLoader.LoadAsync(ls, loadOptions);
        //    }
        //}
    }
}