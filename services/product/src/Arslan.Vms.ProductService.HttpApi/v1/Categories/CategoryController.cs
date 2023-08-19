using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.ProductService.v1.Categories
{
    [ApiController]
    //[RemoteService(Name = "ArslanVmsInventory")]
    [Area("Base")]
    [ControllerName("Category")]
    [Route("rest/api/latest/vms/base/product/category")]
    //[ApiVersion("1.0")]
    public class CategoryController : ProductServiceController, ICategoryAppService
    {
        protected ICategoryAppService _productCategoryAppService { get; }

        public CategoryController(ICategoryAppService productAppService)
        {
            _productCategoryAppService = productAppService;
        }

        [HttpPost]
        public Task<CategoryDto> CreateAsync(CreateCategoryDto input)
        {
            return _productCategoryAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto input)
        {
            return _productCategoryAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _productCategoryAppService.DeleteAsync(id);
        }

        [HttpPost]
        [Route("Undo/{id}")]
        public Task UndoAsync(Guid id)
        {
            return _productCategoryAppService.UndoAsync(id);
        }

        [HttpGet]
        [Route("{id}/{isDeleted}")]
        public Task<CategoryDto> GetAsync(Guid id, bool isDeleted = false)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{withDeleted}")]
        public virtual Task<List<CategoryDto>> GetListAsync(bool withDeleted)
        {
            return _productCategoryAppService.GetListAsync(withDeleted);
        }

        //[HttpGet]
        //[Route("WithDeleteds")]
        //public virtual Task<List<CategoryDto>> GetWithDeletedListAsync()
        //{
        //    return _productCategoryAppService.GetListAsync(true);
        //}


    }
}