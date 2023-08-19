using Arslan.Vms.InventoryService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Guids;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.InventoryService.StockAdjustments
{
    public class StockAdjustmentRepository : EfCoreRepository<IInventoryServiceDbContext, StockAdjustment, Guid>, IStockAdjustmentRepository
    {
        private readonly IObjectMapper _objectMapper;
        private readonly IGuidGenerator _guidGenerator;

        IDbContextProvider<IInventoryServiceDbContext> _dbContextProvider;

        public StockAdjustmentRepository(IDbContextProvider<IInventoryServiceDbContext> dbContextProvider,
            IObjectMapper objectMapper,
            IGuidGenerator guidGenerator) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _objectMapper = objectMapper;
            _guidGenerator = guidGenerator;
        }

        public override IQueryable<StockAdjustment> WithDetails()
        {
            return base.WithDetails()
                .Include(x => x.Lines);
        }
    }
}