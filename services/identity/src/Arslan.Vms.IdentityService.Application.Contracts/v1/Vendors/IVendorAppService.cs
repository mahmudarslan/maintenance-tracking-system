using Arslan.Vms.IdentityService.v1.Vendors.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.IdentityService.v1.Vendors
{
    public interface IVendorAppService
    {
        Task<VendorDto> CreateAsync(CreateVendorDto input);
        Task<VendorDto> UpdateAsync(Guid id, UpdateVendorDto input);
        Task DeleteAsync(string key);
        Task UndoAsync(Guid id);
        Task<VendorDto> GetAsync(Guid id, bool isDeleted = false);
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
        Task<List<VendorSearchDto>> GetBySearchContent(string nameSurnameGsm);
        Task<VendorSearchDto> GetByVendorId(Guid id);
    }
}