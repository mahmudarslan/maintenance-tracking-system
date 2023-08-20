using Arslan.Vms.ProductService.v1.Products.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.ProductService.v1.Products
{
    [ApiVersion("1.0")]
    [ApiController]
    [Area("Base")]
    [ControllerName("Product")]
    [Route("product/v{version:apiVersion}/product")]
    public class ProductController : ProductServiceController, IProductAppService
    {
        protected IProductAppService _productAppService { get; }

        public ProductController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }

        [HttpPost]
        public Task<ProductDto> CreateAsync(CreateProductDto input)
        {
            ValidateModel();
            return _productAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto input)
        {
            return _productAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public virtual Task DeleteAsync([FromForm(Name = "key")] string key)
        {
            return _productAppService.DeleteAsync(key);
        }

        [HttpPost]
        [Route("Undo")]
        public virtual Task UndoAsync(Guid id)
        {
            return _productAppService.UndoAsync(id);
        }

        [HttpGet]
        [Route("{id}/{isDeleted}")]
        public virtual Task<ProductDto> GetAsync(Guid id, bool isDeleted = false)
        {
            return _productAppService.GetAsync(id, isDeleted);
        }

        [HttpGet]
        [Route("{id}/{versionNumber}")]
        public Task<ProductDto> GetVersionAsync(Guid id, int versionNumber)
        {
            return _productAppService.GetVersionAsync(id, versionNumber);
        }

        [HttpGet]
        public virtual Task<LoadResult> GetListAsync(DataSourceLoadOptions input)
        {
            return _productAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("AllService")]
        public virtual Task<List<ProductItemDto>> GetAllServiceAsync()
        {
            return _productAppService.GetAllServiceAsync();
        }

        [HttpGet]
        [Route("AllStockedProduct")]
        public virtual Task<List<ProductItemDto>> GetAllStockedProductAsync()
        {
            return _productAppService.GetAllStockedProductAsync();
        }

        [HttpGet]
        [Route("AllProduct")]
        public virtual Task<List<ProductItemDto>> GetAllProductAsync()
        {
            return _productAppService.GetAllProductAsync();
        }

        [HttpGet]
        [Route("ProductTypes")]
        public Task<List<ProductTypeDto>> GetProductTypeListAsync()
        {
            return _productAppService.GetProductTypeListAsync();
        }
    }
}