using Arslan.Vms.IdentityService.Addresses;
using Arslan.Vms.IdentityService.Addresses.AddressTypes;
using Arslan.Vms.IdentityService.Companies;
using Arslan.Vms.IdentityService.Files;
using Arslan.Vms.IdentityService.Users;
using Arslan.Vms.IdentityService.v1.Files.Dtos;
using Arslan.Vms.IdentityService.v1.Addresses.Dtos;
using Arslan.Vms.IdentityService.v1.AddressTypes.Dto;
using Arslan.Vms.IdentityService.v1.Company.Dtos;
using Arslan.Vms.IdentityService.v1.Customers.Dtos;
using Arslan.Vms.IdentityService.v1.Employee.Dtos;
using Arslan.Vms.IdentityService.v1.Vendors.Dtos;
using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Arslan.Vms.IdentityService;

public class IdentityServiceApplicationAutoMapperProfile : Profile
{
    public IdentityServiceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        #region Vendor
        CreateMap<User, VendorDto>()
                .Ignore(i => i.Addresses);
        #endregion

        #region Employee
        CreateMap<User, EmployeeDto>()
               .Ignore(i => i.RoleId)
               .Ignore(i => i.IsCustomerRole)
               .Ignore(i => i.Addresses);
        #endregion

        #region Customer
        CreateMap<User, CustomerDto>()
                .Ignore(i => i.Addresses)
                //.Ignore(i => i.Vehicles)
                ;
        #endregion

        #region Company
        CreateMap<Company, CompanyDto>()
             .Ignore(i => i.Addresses)
             .Ignore(i => i.Attachments);
        #endregion

        #region Address
        CreateMap<Address, AddressDto>();
        #endregion

        #region AddressType
        CreateMap<AddressType, AddressTypeDto>();
        #endregion

        #region Attachment
        CreateMap<FileAttachment, FileAttachmentDto>();
        #endregion
    }
}
