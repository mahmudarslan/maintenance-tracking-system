using Arslan.Vms.ProductService.v1.Products.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.ProductService.v1.Products
{
	public interface IProductAppService
    {
        Task<ProductDto> CreateAsync(CreateProductDto input);
        Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto input);
        Task DeleteAsync(string key);
        Task UndoAsync(Guid id);
        Task<ProductDto> GetAsync(Guid id, bool isDeleted = false);
        Task<ProductDto> GetVersionAsync(Guid id, int version);
        Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions);
        Task<List<ProductItemDto>> GetAllServiceAsync();
        Task<List<ProductItemDto>> GetAllStockedProductAsync();
        Task<List<ProductItemDto>> GetAllProductAsync();
        Task<List<ProductTypeDto>> GetProductTypeListAsync();
    }
}