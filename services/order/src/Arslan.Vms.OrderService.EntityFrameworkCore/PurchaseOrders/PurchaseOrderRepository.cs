using Arslan.Vms.OrderService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Arslan.Vms.OrderService.PurchaseOrders
{
    public class PurchaseOrderRepository : EfCoreRepository<IOrderServiceDbContext, PurchaseOrder, Guid>, IPurchaseOrderRepository
    {
        //private readonly IRepository<Product, Guid> _productRepository;
        private readonly IDataFilter _dataFilter;

        public PurchaseOrderRepository(IDbContextProvider<IOrderServiceDbContext> dbContextProvider,
            //IRepository<Product, Guid> productRepository,
            IDataFilter dataFilter) : base(dbContextProvider)
        {
            //_productRepository = productRepository;
            _dataFilter = dataFilter;
        }

        public override IQueryable<PurchaseOrder> WithDetails()
        {
            return base.WithDetails(w => w.OrderLines, a => a.ReceiveLines);
        }

        public async Task<PurchaseOrder> GetWithDeatilsAsync(Guid id)
        {
            var po = WithDetails().AsNoTracking().Where(w => w.Id == id).FirstOrDefault();

            if (po == null)
            {
                return null;
            }

            using (_dataFilter.Disable<ISoftDelete>())
            {
                //foreach (var line in po.ReceiveLines)
                //{
                //    line.Product = await _productRepository.FirstOrDefaultAsync(f => f.Id == line.ProductId);
                //}
            }

            return await Task.FromResult(po);
        }
    }
}