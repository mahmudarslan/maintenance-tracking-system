using Arslan.Vms.IdentityService.Addresses;
using Arslan.Vms.IdentityService.Companies;
using Arslan.Vms.IdentityService.Files;
using Arslan.Vms.IdentityService.Permissions;
using Arslan.Vms.IdentityService.v1.Files.Dtos;
using Arslan.Vms.IdentityService.v1.Addresses.Dtos;
using Arslan.Vms.IdentityService.v1.Company;
using Arslan.Vms.IdentityService.v1.Company.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.IdentityService.v1.Company
{
    [Authorize(AdministrationServicePermissions.Company.Default)]
    public class CompanyAppService : AdministrationServiceAppService, ICompanyAppService
    {
        #region Fields
        private readonly ICompanyRepository _companyRepository;
        private readonly IRepository<FileAttachment, Guid> _fileAttachmentRepository;
        private readonly IRepository<Address, Guid> _addressRepository;
        //private readonly IStringLocalizer<VehicleServiceResource> _localizer;
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IDataFilter _dataFilter;
        #endregion

        #region Ctor
        public CompanyAppService(ICompanyRepository companyRepository,
                        IRepository<FileAttachment, Guid> fileAttachmentRepository,
                        IRepository<Address, Guid> addressRepository,
                        ICurrentTenant currentTenant,
                        //IStringLocalizer<VehicleServiceResource> localizer,
                        IDataFilter dataFilter,
                        IGuidGenerator guidGenerator
                         )
        {

            _companyRepository = companyRepository;
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            //_localizer = localizer;
            _dataFilter = dataFilter;
            _fileAttachmentRepository = fileAttachmentRepository;
            _addressRepository = addressRepository;
        }
        #endregion

        [Authorize(AdministrationServicePermissions.Company.Update)]
        public async Task<CompanyDto> UpdateAsync(CreateUpdateCompanyDto input)
        {
            var companyRepository = await _companyRepository.GetQueryableAsync();
            var fileAttachmentRepository = await _fileAttachmentRepository.GetQueryableAsync();


            var files = fileAttachmentRepository.Where(w => w.DownloadGuid == input.FakeId).ToList();

            var company = await _companyRepository.FirstOrDefaultAsync(f => f.TenantId == _currentTenant.Id);
            company.SetName(input.Name);
            company.SetTaxNumber(input.TaxNumber);
            company.SetWebsiteUrl(input.WebsiteUrl);
            company.SetFaxNumber(input.FaxNumber);
            company.SetEmail(input.Email);
            company.SetPhoneNumber(input.PhoneNumber);
            company.LogoAttachmentId = files.FirstOrDefault()?.Id;

            foreach (var inputAddress in input.Addresses ?? Enumerable.Empty<UpdateAddressDto>())
            {
                if (inputAddress.Id == Guid.Empty)
                {
                    var insertedAddress = new Address(_guidGenerator.Create(),
                        _currentTenant.Id, inputAddress.AddressType, 1,
                        inputAddress.CountryId, inputAddress.CityId, inputAddress.DistrictId,
                        inputAddress.AddressName, inputAddress.Address1, inputAddress.Address2,
                        inputAddress.PostalCode, inputAddress.Remarks)
                    {
                        CityName = inputAddress.CityName,
                        CountryName = inputAddress.CountryName,
                        DistrictName = inputAddress.DistrictName,
                        IsBillingAddress = inputAddress.IsBillingAddress,
                        IsDefaultAddress = inputAddress.IsDefaultAddress,
                        IsShippingAddress = inputAddress.IsShippingAddress
                    };

                    company.AddAddress(insertedAddress.Id);
                }
                else
                {
                    var updateUserAddress = await _addressRepository.FirstOrDefaultAsync(f => f.Id == inputAddress.Id);
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
                }
            }

            #region  Attachment
            //Delete old uploaded files
            var oldFiles = (from company1 in companyRepository
                            from file in fileAttachmentRepository
                            from rfile in company1.Attachments
                            where rfile.CompanyId == company.Id && company1.Id == company.Id
                            select file
                 ).ToList();

            foreach (var oldFile in oldFiles)
            {
                oldFile.IsDeleted = true;
                await _fileAttachmentRepository.UpdateAsync(oldFile, true);
            }

            //Insert Attachment Files
            foreach (var file in files ?? Enumerable.Empty<FileAttachment>())
            {
                company.AddAttachment(file.Id);
            }
            #endregion

            await _companyRepository.UpdateAsync(company);

            return new CompanyDto
            {
                Id = company.Id,
                Name = company.Name,
                Email = company.Email,
                FaxNumber = company.FaxNumber,
                PhoneNumber = company.PhoneNumber,
                TaxNumber = company.TaxNumber,
                WebsiteUrl = company.WebsiteUrl,
                //Addresses = ObjectMapper.Map<List<Address>, List<AddressDto>>(company.Addresses.Select(s => s.Address).ToList()),
                //Attachments = ObjectMapper.Map<List<FileAttachment>, List<FileAttachmentDto>>(company.Attachments.Select(s => s.FileAttachments).ToList())
            };
        }

        public async Task<CompanyDto> GetAsync(Guid id)
        {
            var company = await _companyRepository.GetAsync(id);
            var address = await _companyRepository.GetAddresses(id);
            var attachments = await _companyRepository.GetAttachmets(id);

            return new CompanyDto
            {
                Name = company.Name,
                Email = company.Email,
                FaxNumber = company.FaxNumber,
                PhoneNumber = company.PhoneNumber,
                TaxNumber = company.TaxNumber,
                WebsiteUrl = company.WebsiteUrl,
                Addresses = ObjectMapper.Map<List<Address>, List<AddressDto>>(address),
                Attachments = ObjectMapper.Map<List<FileAttachment>, List<FileAttachmentDto>>(attachments)
            };
        }
    }
}