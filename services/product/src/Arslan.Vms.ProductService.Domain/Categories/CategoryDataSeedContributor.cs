using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.ProductService.Categories
{
    public class CategoryDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Category, Guid> _productCategoryRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public CategoryDataSeedContributor(
            IRepository<Category, Guid> productCategoryRepository,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _productCategoryRepository = productCategoryRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await CreateCategory();
        }

        async Task<List<Category>> CreateCategory()
        {
            var productCategorList = _productCategoryRepository.GetListAsync().Result;

            if (!productCategorList.Select(s => s.Name).Contains("Default Category"))
            {
                var productCategory = new Category(_guidGenerator.Create(), _currentTenant.Id, null, "Default Category", 1);
                productCategorList.Add(productCategory);
                await _productCategoryRepository.InsertAsync(productCategory);
            }

            return productCategorList;
        }
    }
}