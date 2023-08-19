using Arslan.Vms.ProductService.Products.Versions;
using Arslan.Vms.ProductService.Products;
using Arslan.Vms.ProductService.Taxes;
using AutoMapper;
using System;
using Volo.Abp.AutoMapper;
using Arslan.Vms.ProductService.v1.Products.Dtos;
using Arslan.Vms.ProductService.v1.Pricing.Dtos;
using Arslan.Vms.ProductService.v1.Taxes.Dtos;
using Arslan.Vms.ProductService.v1.Files.Dtos;
using Arslan.Vms.ProductService.Files; 
using Arslan.Vms.ProductService.v1.Categories;
using Arslan.Vms.ProductService.v1.Locations.Dtos;
using Arslan.Vms.ProductService.v1.AddressTypes.Dto;
using Arslan.Vms.ProductService.v1.Address.Dtos;
using Arslan.Vms.ProductService.v1.Vendors.Dtos;
using Arslan.Vms.ProductService.Addresses.AddressTypes;
using Arslan.Vms.ProductService.Categories;
using Arslan.Vms.ProductService.Locations;
using Arslan.Vms.ProductService.Addresses;
using Arslan.Vms.ProductService.Pricing;

namespace Arslan.Vms.ProductService;

public class ProductServiceApplicationAutoMapperProfile : Profile
{
    public ProductServiceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        //#region Vendor
        //CreateMap<User, VendorDto>()
        //        .Ignore(i => i.Addresses);
        //#endregion
         
        #region Address
        CreateMap<Address, AddressDto>();
        #endregion

        #region AddressType
        CreateMap<AddressType, AddressTypeDto>();
        #endregion

 

        #region Category
        CreateMap<Category, CategoryDto>();
        #endregion

        #region Location
        CreateMap<Location, LocationDto>();
        #endregion

        #region Product
        CreateMap<Product, ProductDto>()
            .Ignore(i => i.Attachments);
        CreateMap<ProductPrice, ProductPriceDto>();

        CreateMap<ProductVersion, ProductDto>()
            .Ignore(i => i.Attachments)
            .Ignore(i => i.Prices)
            .ForMember(f => f.Id, opt => opt.MapFrom(src => src.ProductId));

        CreateMap<Product, ProductVersion>()
            .ForMember(f => f.Id, opt => opt.MapFrom(src => Guid.Empty))
            .ForMember(f => f.ProductId, opt => opt.MapFrom(src => src.Id));

        CreateMap<ProductPrice, ProductPriceVersion>()
            .ForMember(f => f.Id, opt => opt.MapFrom(src => Guid.Empty))
            .ForMember(f => f.ProductPriceId, opt => opt.MapFrom(src => src.Id));

        CreateMap<ProductPriceVersion, ProductPriceDto>()
              .ForMember(f => f.Id, opt => opt.MapFrom(src => src.ProductPriceId));
        #endregion

        #region Pricing
        CreateMap<PricingScheme, PricingSchemeDto>();
        #endregion

        #region Taxing
        CreateMap<TaxingScheme, TaxingSchemeDto>();
        CreateMap<TaxCode, TaxCodeDto>();
        #endregion

        #region Attachment
        CreateMap<FileAttachment, FileAttachmentDto>();
        #endregion
    }
}
