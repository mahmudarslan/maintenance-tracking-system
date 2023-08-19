using Arslan.Vms.IdentityService;
using Arslan.Vms.IdentityService.Permissions;
using Microsoft.AspNetCore.Authorization;

namespace Arslan.Vms.IdentityService.v1.Vendors
{
    [Authorize(IdentityServicePermissions.Vendor.Default)]
    public class VendorAppService : IdentityServiceAppService //, IVendorAppService
    {
        //#region Fields
        //private IIdentityUserAppService _identityUserAppService;
        //private IAppUserRepository _appUserRepository;
        //private IAppUserRoleRepository _userRoleRepository;
        //private IAppRoleRepository _roleRepository;
        ////private IRepository<UserAddress, Guid> _userAddressRepository;
        //private IStringLocalizer<BaseResource> _localizer;
        //private ICurrentTenant _currentTenant;
        //private IGuidGenerator _guidGenerator;
        //private IDataFilter _dataFilter;
        //#endregion

        //#region Ctor
        //public VendorAppService(
        //                //IRepository<UserAddress, Guid> userAddressRepository,
        //                IIdentityUserAppService identityUserAppService,
        //                IAppUserRepository appUserRepository,
        //                IAppRoleRepository roleRepository,
        //                IAppUserRoleRepository userRoleRepository,
        //                ICurrentTenant currentTenant,
        //                IStringLocalizer<BaseResource> localizer,
        //                IDataFilter dataFilter,
        //                IGuidGenerator guidGenerator
        //                 )
        //{

        //    //_userAddressRepository = userAddressRepository;
        //    _identityUserAppService = identityUserAppService;
        //    _appUserRepository = appUserRepository;
        //    _roleRepository = roleRepository;
        //    _userRoleRepository = userRoleRepository;
        //    _currentTenant = currentTenant;
        //    _guidGenerator = guidGenerator;
        //    _localizer = localizer;
        //    _dataFilter = dataFilter;
        //}
        //#endregion      

        //[Authorize(VehicleServicePermissions.Vendor.Create)]
        //public async Task<VendorDto> CreateAsync(CreateVendorDto input)
        //{
        //    var user = new IdentityUserCreateDto()
        //    {
        //        UserName = input.UserName,
        //        Email = input.Email,
        //        Name = input.Name,
        //        Surname = input.Surname,
        //        Password = "Demo_123*",
        //        PhoneNumber = input.PhoneNumber,
        //        RoleNames = new string[] { RoleConsts.Vendor },
        //    };
        //    var identityUser = await _identityUserAppService.CreateAsync(user);

        //    var updatedVendor = await _appUserRepository.GetAsync(identityUser.Id);
        //    updatedVendor.HomePhoneNumber = input.HomePhoneNumber;
        //    updatedVendor.FaxNumber = input.FaxNumber;
        //    updatedVendor.BirthDate = input.BirthDate;
        //    updatedVendor.WorkPhoneNumber = input.WorkPhoneNumber;


        //    var addresses = new List<Address>();

        //    foreach (var inputAddress in input.Addresses ?? Enumerable.Empty<CreateAddressDto>())
        //    {
        //        var insertedAddress = new Address(_guidGenerator.Create(), _currentTenant.Id, inputAddress.AddressType, 1,
        //            inputAddress.CountryId, inputAddress.CityId, inputAddress.DistrictId,
        //                inputAddress.AddressName, inputAddress.Address1, inputAddress.Address2, inputAddress.PostalCode, inputAddress.Remarks);

        //        insertedAddress.CountryName = inputAddress.CountryName;
        //        insertedAddress.CityName = inputAddress.CityName;
        //        insertedAddress.DistrictName = inputAddress.DistrictName;
        //        insertedAddress.IsDefaultAddress = inputAddress.IsDefaultAddress;
        //        insertedAddress.IsBillingAddress = inputAddress.IsBillingAddress;
        //        insertedAddress.IsShippingAddress = inputAddress.IsShippingAddress;

        //        updatedVendor.AddAddress(insertedAddress);

        //        addresses.Add(insertedAddress);
        //    }

        //    await _appUserRepository.UpdateAsync(updatedVendor, true);

        //    var result = ObjectMapper.Map<AppUser, VendorDto>(updatedVendor);
        //    result.Addresses = ObjectMapper.Map<List<Address>, List<AddressDto>>(addresses);

        //    return result;
        //}

        //[Authorize(VehicleServicePermissions.Vendor.Update)]
        //public async Task<VendorDto> UpdateAsync(Guid id, UpdateVendorDto input)
        //{
        //    var identityUser = await _appUserRepository.GetAsync(id);
        //    identityUser.HomePhoneNumber = input.HomePhoneNumber;
        //    identityUser.FaxNumber = input.FaxNumber;
        //    identityUser.BirthDate = input.BirthDate;
        //    identityUser.WorkPhoneNumber = input.WorkPhoneNumber;



        //    var addresses = new List<Address>();

        //    foreach (var inputAddress in input.Addresses ?? Enumerable.Empty<UpdateAddressDto>())
        //    {
        //        if (inputAddress.Id == Guid.Empty)
        //        {
        //            var insertedAddress = new Address(_guidGenerator.Create(), _currentTenant.Id, inputAddress.AddressType, 1,
        //                inputAddress.CountryId, inputAddress.CityId, inputAddress.DistrictId,
        //                inputAddress.AddressName, inputAddress.Address1, inputAddress.Address2,
        //                inputAddress.PostalCode, inputAddress.Remarks);

        //            insertedAddress.CountryName = inputAddress.CountryName;
        //            insertedAddress.CityName = inputAddress.CityName;
        //            insertedAddress.DistrictName = inputAddress.DistrictName;
        //            insertedAddress.IsDefaultAddress = inputAddress.IsDefaultAddress;
        //            insertedAddress.IsBillingAddress = inputAddress.IsBillingAddress;
        //            insertedAddress.IsShippingAddress = inputAddress.IsShippingAddress;

        //            identityUser.AddAddress(insertedAddress);

        //            addresses.Add(insertedAddress);
        //        }
        //        else
        //        {
        //            var updateUserAddress = (await _appUserRepository.GetAddresses(id)).FirstOrDefault(f => f.Id == inputAddress.Id);
        //            updateUserAddress.Address1 = inputAddress.Address1;
        //            updateUserAddress.Address2 = inputAddress.Address2;
        //            updateUserAddress.AddressName = inputAddress.AddressName;
        //            updateUserAddress.AddressType = inputAddress.AddressType;
        //            updateUserAddress.CityId = inputAddress.CityId;
        //            updateUserAddress.CountryId = inputAddress.CountryId;
        //            updateUserAddress.DistrictId = inputAddress.DistrictId;
        //            updateUserAddress.CityName = inputAddress.CityName;
        //            updateUserAddress.CountryName = inputAddress.CountryName;
        //            updateUserAddress.DistrictName = inputAddress.DistrictName;
        //            updateUserAddress.IsBillingAddress = inputAddress.IsBillingAddress;
        //            updateUserAddress.IsDefaultAddress = inputAddress.IsDefaultAddress;
        //            updateUserAddress.IsShippingAddress = inputAddress.IsShippingAddress;
        //            updateUserAddress.PostalCode = inputAddress.PostalCode;
        //            updateUserAddress.Remarks = inputAddress.Remarks;
        //            addresses.Add(updateUserAddress);
        //        }
        //    }

        //    await _appUserRepository.UpdateAsync(identityUser, true);

        //    var identityUserDto = new IdentityUserUpdateDto
        //    {
        //        Name = input.Name,
        //        UserName = input.UserName,
        //        Email = input.Email,
        //        Surname = input.Surname,
        //        PhoneNumber = input.PhoneNumber,
        //        ConcurrencyStamp = identityUser.ConcurrencyStamp
        //    };
        //    await _identityUserAppService.UpdateAsync(id, identityUserDto);

        //    var result = ObjectMapper.Map<AppUser, VendorDto>(identityUser);
        //    result.Addresses = ObjectMapper.Map<List<Address>, List<AddressDto>>(addresses);

        //    return result;
        //}

        //[Authorize(VehicleServicePermissions.Vendor.Delete)]
        //public async Task DeleteAsync(string key)
        //{
        //    var user = await _appUserRepository.GetAsync(Guid.Parse(key));
        //    await _appUserRepository.DeleteAsync(user, true);
        //}

        //[Authorize(VehicleServicePermissions.Vendor.Undo)]
        //public async Task UndoAsync(Guid id)
        //{
        //    using (_dataFilter.Disable<ISoftDelete>())
        //    {
        //        var user = await _appUserRepository.FirstOrDefaultAsync(f => f.Id == id);
        //        user.IsDeleted = false;
        //        await _appUserRepository.UpdateAsync(user, true);
        //    }
        //}

        //public async Task<VendorDto> GetAsync(Guid id, bool isDeleted)
        //{
        //    using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
        //    {
        //        var appUserRepository = await _appUserRepository.GetQueryableAsync();
        //        var userRoleRepository = await _userRoleRepository.GetQueryableAsync();
        //        var roleRepository = await _roleRepository.GetQueryableAsync();

        //        var appUser = (from user in appUserRepository
        //                       from userRole in userRoleRepository.Where(w => w.UserId == user.Id)
        //                       from role in roleRepository.Where(w => w.Id == userRole.RoleId && (
        //                       w.NormalizedName == RoleConsts.Vendor))
        //                       where user.Id == id
        //                       select new VendorDto
        //                       {
        //                           Id = user.Id,
        //                           UserName = user.UserName,
        //                           Name = user.Name,
        //                           Surname = user.Surname,
        //                           BirthDate = user.BirthDate,
        //                           PhoneNumber = user.PhoneNumber,
        //                           HomePhoneNumber = user.HomePhoneNumber,
        //                           WorkPhoneNumber = user.WorkPhoneNumber,
        //                           FaxNumber = user.FaxNumber,
        //                           CreationTime = user.CreationTime,
        //                           LastModificationTime = user.LastModificationTime,
        //                           Email = user.Email,
        //                           IsDeleted = user.IsDeleted
        //                       }).FirstOrDefault();

        //        var addresses = await _appUserRepository.GetAddresses(id);

        //        appUser.Addresses = ObjectMapper.Map<List<Address>, List<AddressDto>>(addresses);

        //        return await Task.FromResult(appUser);
        //    }
        //}

        //[Authorize(VehicleServicePermissions.Vendor.List)]
        //public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        //{
        //    loadOptions.PrimaryKey = new[] { "Id" };

        //    using (_dataFilter.Disable<ISoftDelete>())
        //    {
        //        var appUserRepository = await _appUserRepository.GetQueryableAsync();
        //        var userRoleRepository = await _userRoleRepository.GetQueryableAsync();
        //        var roleRepository = await _roleRepository.GetQueryableAsync();

        //        var ls = (from user in appUserRepository
        //                  from userRole in userRoleRepository.Where(w => w.UserId == user.Id)
        //                  from role in roleRepository.Where(w => w.Id == userRole.RoleId &&
        //                  (w.NormalizedName == RoleConsts.Vendor))
        //                  select new VendorDto
        //                  {
        //                      Id = user.Id,
        //                      UserName = user.UserName,
        //                      Name = user.Name,
        //                      Surname = user.Surname,
        //                      BirthDate = user.BirthDate,
        //                      PhoneNumber = user.PhoneNumber,
        //                      HomePhoneNumber = user.HomePhoneNumber,
        //                      WorkPhoneNumber = user.WorkPhoneNumber,
        //                      FaxNumber = user.FaxNumber,
        //                      Email = user.Email,
        //                      CreationTime = user.CreationTime,
        //                      LastModificationTime = user.LastModificationTime,
        //                      IsDeleted = user.IsDeleted
        //                  });

        //        return await DataSourceLoader.LoadAsync(ls, loadOptions);
        //    }
        //}

        //public async Task<List<VendorSearchDto>> GetBySearchContent(string nameSurnameGsm)
        //{
        //    var appUserRepository = await _appUserRepository.GetQueryableAsync();
        //    var userRoleRepository = await _userRoleRepository.GetQueryableAsync();
        //    var roleRepository = await _roleRepository.GetQueryableAsync();

        //    var result = (from user in appUserRepository
        //                  from userRole in userRoleRepository.Where(w => w.UserId == user.Id)
        //                  from role in roleRepository.Where(w => w.Id == userRole.RoleId && w.NormalizedName == RoleConsts.Vendor)
        //                  where (
        //                  (user.Name + " " + user.Surname).Contains(nameSurnameGsm) ||
        //                  user.PhoneNumber.Contains(nameSurnameGsm))
        //                  select new VendorSearchDto
        //                  {
        //                      Id = user.Id,
        //                      Name = user.Name,
        //                      Surname = user.Surname,
        //                      PhoneNumber = user.PhoneNumber,
        //                      SelectionField = $"{user.Name} {user.Surname} {user.PhoneNumber}"
        //                  }
        //                    ).ToList();

        //    return await Task.FromResult(result);
        //}

        //public async Task<VendorSearchDto> GetByVendorId(Guid id)
        //{
        //    var appUserRepository = await _appUserRepository.GetQueryableAsync();
        //    var userRoleRepository = await _userRoleRepository.GetQueryableAsync();
        //    var roleRepository = await _roleRepository.GetQueryableAsync();

        //    var result = (from user in appUserRepository
        //                  from userRole in userRoleRepository.Where(w => w.UserId == user.Id)
        //                  from role in roleRepository.Where(w => w.Id == userRole.RoleId && w.NormalizedName == RoleConsts.Vendor)
        //                  where user.Id == id
        //                  select new VendorSearchDto
        //                  {
        //                      Id = user.Id,
        //                      Name = user.Name,
        //                      Surname = user.Surname,
        //                      PhoneNumber = user.PhoneNumber,
        //                      SelectionField = $"{user.Name} {user.Surname} {user.PhoneNumber}"
        //                  }
        //                    ).FirstOrDefault();

        //    return await Task.FromResult(result);
        //}
    }
}