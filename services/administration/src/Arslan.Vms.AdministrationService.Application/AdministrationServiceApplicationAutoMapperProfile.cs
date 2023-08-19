using Arslan.Vms.AdministrationService.Addresses;
using Arslan.Vms.AdministrationService.Addresses.AddressTypes;
using Arslan.Vms.AdministrationService.Companies;
using Arslan.Vms.AdministrationService.Files;
using Arslan.Vms.AdministrationService.Users;
using Arslan.Vms.AdministrationService.v1.Files.Dtos;
using Arslan.Vms.AdministrationService.v1.Addresses.Dtos;
using Arslan.Vms.AdministrationService.v1.AddressTypes.Dto;
using Arslan.Vms.AdministrationService.v1.Company.Dtos;
using Arslan.Vms.AdministrationService.v1.Customers.Dtos;
using Arslan.Vms.AdministrationService.v1.Employee.Dtos;
using Arslan.Vms.AdministrationService.v1.Vendors.Dtos;
using AutoMapper;
using Volo.Abp.AutoMapper;

namespace Arslan.Vms.AdministrationService;

public class AdministrationServiceApplicationAutoMapperProfile : Profile
{
    public AdministrationServiceApplicationAutoMapperProfile()
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
