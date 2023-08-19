using Arslan.Vms.AdministrationService.Addresses;
using Arslan.Vms.AdministrationService.Permissions;
using Arslan.Vms.AdministrationService.Roles;
using Arslan.Vms.AdministrationService.Users;
using Arslan.Vms.AdministrationService.v1.Addresses.Dtos;
using Arslan.Vms.AdministrationService.v1.Employee;
using Arslan.Vms.AdministrationService.v1.Employee.Dtos;
using Arslan.Vms.AdministrationService.v1.Users.Dtos;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.AdministrationService.v1.Employees
{
    [Authorize(AdministrationServicePermissions.Employee.Default)]
    public class EmployeeAppService : AdministrationServiceAppService, IEmployeeAppService
    {
        #region Fields
        //private IIdentityUserAppService _identityUserAppService;
        //private IIdentityUserRepository _identityUserRepository;
        private readonly IRepository<User, Guid> _appUserRepository;
        private readonly IRepository<Role, Guid> _appRoleRepository;
        //private readonly IRepository<UserRole, Guid> _appUserRoleRepository;
        private readonly IUserManager _userManager;
        //private IAppUserRepository _appUserRepository;
        //private IAppUserRoleRepository _userRoleRepository;
        //private IAppRoleRepository _roleRepository;
        //private readonly IStringLocalizer<VehicleServiceResource> _localizer;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDataFilter _dataFilter;
        #endregion

        #region Ctor
        public EmployeeAppService(
                IRepository<User, Guid> appUserRepository,
                IRepository<Role, Guid> appRoleRepository,
                //IRepository<UserRole, Guid> appUserRoleRepository,
                //IIdentityUserAppService identityUserAppService,
                //IAppUserRepository appUserRepository,
                //IAppRoleRepository roleRepository,
                //IAppUserRoleRepository userRoleRepository,
                IUserManager userManager,
                ICurrentTenant currentTenant,
                //IStringLocalizer<VehicleServiceResource> localizer,
                IDataFilter dataFilter,
                IGuidGenerator guidGenerator
                )
        {
            //_identityUserAppService = identityUserAppService;
            //_appUserRepository = appUserRepository;
            //_roleRepository = roleRepository;
            //_userRoleRepository = userRoleRepository;

            _appRoleRepository = appRoleRepository;
            _appUserRepository = appUserRepository;
            //_appUserRoleRepository = appUserRoleRepository;
            _userManager = userManager;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            //_localizer = localizer;
            _dataFilter = dataFilter;
        }
        #endregion

        [Authorize(AdministrationServicePermissions.Employee.Create)]
        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto input)
        {
            var user = new User(GuidGenerator.Create(), CurrentTenant.Id, input.UserName, input.Name, input.Surname, input.Email)
            {
                //Password = "Demo_123*",
                //RoleNames = CheckRoles(input.Roles),
                HomePhoneNumber = input.HomePhoneNumber,
                FaxNumber = input.FaxNumber,
                BirthDate = input.BirthDate,
                WorkPhoneNumber = input.WorkPhoneNumber,
            };

            var addresses = new List<Address>();

            foreach (var inputAddress in input.Addresses ?? Enumerable.Empty<CreateAddressDto>())
            {
                var insertedAddress = new Address(_guidGenerator.Create(), _currentTenant.Id, inputAddress.AddressType, 1,
                    inputAddress.CountryId, inputAddress.CityId, inputAddress.DistrictId,
                        inputAddress.AddressName, inputAddress.Address1, inputAddress.Address2,
                        inputAddress.PostalCode, inputAddress.Remarks);

                insertedAddress.CountryName = inputAddress.CountryName;
                insertedAddress.CityName = inputAddress.CityName;
                insertedAddress.DistrictName = inputAddress.DistrictName;
                insertedAddress.IsDefaultAddress = inputAddress.IsDefaultAddress;
                insertedAddress.IsBillingAddress = inputAddress.IsBillingAddress;
                insertedAddress.IsShippingAddress = inputAddress.IsShippingAddress;

                user.AddAddress(insertedAddress.Id);

                addresses.Add(insertedAddress);
            }

            input.Roles.ForEach(f =>
            {
                user.AddRole(f);
            });


            await _userManager.CreateAsync(user);

            var result = ObjectMapper.Map<User, EmployeeDto>(user);
            result.Addresses = ObjectMapper.Map<List<Address>, List<AddressDto>>(addresses);

            return result;
        }

        [Authorize(AdministrationServicePermissions.Employee.Update)]
        public async Task<EmployeeDto> UpdateAsync(Guid id, UpdateEmployeeDto input)
        {
            var identityUser = await _appUserRepository.GetAsync(id);
            identityUser.HomePhoneNumber = input.HomePhoneNumber;
            identityUser.FaxNumber = input.FaxNumber;
            identityUser.BirthDate = input.BirthDate;
            identityUser.WorkPhoneNumber = input.WorkPhoneNumber;


            var addresses = new List<Address>();

            foreach (var inputAddress in input.Addresses ?? Enumerable.Empty<UpdateAddressDto>())
            {
                if (inputAddress.Id == Guid.Empty)
                {
                    var insertedAddress = new Address(_guidGenerator.Create(), _currentTenant.Id, inputAddress.AddressType, 1,
                        inputAddress.CountryId, inputAddress.CityId, inputAddress.DistrictId,
                        inputAddress.AddressName, inputAddress.Address1, inputAddress.Address2,
                        inputAddress.PostalCode, inputAddress.Remarks);

                    insertedAddress.CountryName = inputAddress.CountryName;
                    insertedAddress.CityName = inputAddress.CityName;
                    insertedAddress.DistrictName = inputAddress.DistrictName;
                    insertedAddress.IsDefaultAddress = inputAddress.IsDefaultAddress;
                    insertedAddress.IsBillingAddress = inputAddress.IsBillingAddress;
                    insertedAddress.IsShippingAddress = inputAddress.IsShippingAddress;

                    identityUser.AddAddress(insertedAddress.Id);

                    addresses.Add(insertedAddress);
                }
                else
                {
                    //var updateUserAddress = (await _appUserRepository.GetAddresses(id)).FirstOrDefault(f => f.Id == inputAddress.Id);
                    //updateUserAddress.Address1 = inputAddress.Address1;
                    //updateUserAddress.Address2 = inputAddress.Address2;
                    //updateUserAddress.AddressName = inputAddress.AddressName;
                    //updateUserAddress.AddressType = inputAddress.AddressType;
                    //updateUserAddress.CityId = inputAddress.CityId;
                    //updateUserAddress.CountryId = inputAddress.CountryId;
                    //updateUserAddress.DistrictId = inputAddress.DistrictId;
                    //updateUserAddress.CityName = inputAddress.CityName;
                    //updateUserAddress.CountryName = inputAddress.CountryName;
                    //updateUserAddress.DistrictName = inputAddress.DistrictName;
                    //updateUserAddress.IsBillingAddress = inputAddress.IsBillingAddress;
                    //updateUserAddress.IsDefaultAddress = inputAddress.IsDefaultAddress;
                    //updateUserAddress.IsShippingAddress = inputAddress.IsShippingAddress;
                    //updateUserAddress.PostalCode = inputAddress.PostalCode;
                    //updateUserAddress.Remarks = inputAddress.Remarks;
                    //addresses.Add(updateUserAddress);
                }
            }

            await _appUserRepository.UpdateAsync(identityUser, true);

            var user = await _appUserRepository.GetAsync(identityUser.Id);
            user.Name = input.Name;
            user.UserName = input.UserName;
            user.Email = input.Email;
            user.Surname = input.Surname;
            user.PhoneNumber = input.PhoneNumber;


            //var identityUserDto = new IdentityUserUpdateDto
            //{
            //    Name = input.Name,
            //    UserName = input.UserName,
            //    Email = input.Email,
            //    Surname = input.Surname,
            //    PhoneNumber = input.PhoneNumber,
            //    //RoleNames = input.Roles,
            //    ConcurrencyStamp = identityUser.ConcurrencyStamp
            //};
            await _appUserRepository.UpdateAsync(user);

            var result = ObjectMapper.Map<User, EmployeeDto>(identityUser);
            result.Addresses = ObjectMapper.Map<List<Address>, List<AddressDto>>(addresses);

            return result;
        }

        [Authorize(AdministrationServicePermissions.Employee.Delete)]
        public async Task DeleteAsync(string key)
        {
            var user = await _appUserRepository.GetAsync(Guid.Parse(key));
            await _appUserRepository.DeleteAsync(user, true);
        }

        [Authorize(AdministrationServicePermissions.Employee.Undo)]
        public async Task UndoAsync(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var user = await _appUserRepository.FirstOrDefaultAsync(f => f.Id == id);
                user.IsDeleted = false;
                await _appUserRepository.UpdateAsync(user, true);
            }
        }

        public async Task<EmployeeDto> GetAsync(Guid id, bool isDeleted)
        {
            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var user = await _appUserRepository.GetAsync(id);
                //var roles = await _identityUserAppService.GetRolesAsync(id);
                //var addresses = await _appUserRepository.GetAddresses(id);

                var appUser = ObjectMapper.Map<User, EmployeeDto>(user);
                //appUser.Addresses = ObjectMapper.Map<List<Address>, List<AddressDto>>(addresses);
                //appUser.IsCustomerRole = roles.Items.Any(a => a.Name == RoleConsts.Customer.ToLower());
                //appUser.RoleId = roles.Items.FirstOrDefault(f => f.Name != RoleConsts.Customer.ToLower()).Id;

                return appUser;
            }
        }

        [Authorize(AdministrationServicePermissions.Employee.List)]
        public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                loadOptions.PrimaryKey = new[] { "Id" };

                var appUserRepository = await _appUserRepository.GetQueryableAsync();
                //var userRoleRepository = await _appUserRoleRepository.GetQueryableAsync();
                var roleRepository = await _appRoleRepository.GetQueryableAsync();

                var ls = from user in appUserRepository
                          from userRole in user.Roles.Where(w => w.UserId == user.Id)
                          from role in roleRepository.Where(w => w.Id == userRole.RoleId &&
                          (w.NormalizedName == RoleConsts.Technician || w.NormalizedName == RoleConsts.HeadTechnician))
                          select new EmployeeListDto
                          {
                              Id = user.Id,
                              RoleId = role.Id,
                              UserName = user.UserName,
                              Name = user.Name,
                              Surname = user.Surname,
                              BirthDate = user.BirthDate,
                              PhoneNumber = user.PhoneNumber,
                              HomePhoneNumber = user.HomePhoneNumber,
                              WorkPhoneNumber = user.WorkPhoneNumber,
                              FaxNumber = user.FaxNumber,
                              IsCustomerRole = role.NormalizedName == RoleConsts.Customer,
                              Email = user.Email,
                              CreationTime = user.CreationTime,
                              LastModificationTime = user.LastModificationTime,
                              IsDeleted = user.IsDeleted
                          };

                return await DataSourceLoader.LoadAsync(ls, loadOptions);
            }
        }

        public async Task<RoleUserDto> GetEmployee(Guid id)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var s = await _appUserRepository.GetAsync(id);

                return new RoleUserDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Surname = s.Surname,
                    NameSurname = s.Name + " " + s.Surname
                };
            }
        }

        public async Task<List<RoleUserDto>> GetTechnicians()
        {
            var user = await _appUserRepository.GetQueryableAsync();
            var role = await _appRoleRepository.GetQueryableAsync();
            //var user_Role = await _appUserRoleRepository.GetQueryableAsync();

            return (from u in user
                    from ur in u.Roles.Where(w => w.UserId == u.Id)
                    from r in role.Where(w => w.Id == ur.RoleId && w.Name == RoleConsts.Technician.ToUpperInvariant())
                    select new RoleUserDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Surname = u.Surname,
                        NameSurname = u.Name + " " + u.Surname
                    }).ToList();
        }

        public async Task<List<RoleUserDto>> GetHeadTechnicians()
        {
            var user = await _appUserRepository.GetQueryableAsync();
            var role = await _appRoleRepository.GetQueryableAsync();
            //var user_Role = await _appUserRoleRepository.GetQueryableAsync();

            return (from u in user
                    from ur in u.Roles.Where(w => w.UserId == u.Id)
                    from r in role.Where(w => w.Id == ur.RoleId && w.Name == RoleConsts.HeadTechnician.ToUpperInvariant())
                    select new RoleUserDto
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Surname = u.Surname,
                        NameSurname = u.Name + " " + u.Surname
                    }).ToList();
        }

        public async Task<List<RoleDto>> GetRoles()
        {
            var roleRepository = await _appRoleRepository.GetQueryableAsync();

            var data = roleRepository.Where(w => w.NormalizedName == RoleConsts.HeadTechnician || w.NormalizedName == RoleConsts.Technician)
                .Select(s => new RoleDto
                {
                    Id = s.Id,
                    Name = L[s.NormalizedName].Value,
                    Value = s.Id
                }).ToList();
            return await Task.FromResult(data);
        }

        string[] CheckRoles(string[] input)
        {
            var roles = new List<string>();

            if (input.Contains(RoleConsts.Technician))
            {
                roles.Add(RoleConsts.Technician);
            }
            if (input.Contains(RoleConsts.HeadTechnician))
            {
                roles.Add(RoleConsts.HeadTechnician);
            }
            if (input.Contains(RoleConsts.Customer))
            {
                roles.Add(RoleConsts.Customer);
            }

            return roles.ToArray();
        }
    }
}