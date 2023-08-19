using Arslan.Vms.VehicleService.EntityFrameworkCore;
using Arslan.Vms.VehicleService.Vehicles.VehicleTypes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.VehicleService.VehicleTypes
{
    public class VehicleTypeRepository : EfCoreRepository<VehicleServiceDbContext, VehicleType, Guid>, IVehicleTypeRepository
    {
        private readonly IObjectMapper _objectMapper;
        IDbContextProvider<VehicleServiceDbContext> _dbContextProvider;

        public VehicleTypeRepository(IDbContextProvider<VehicleServiceDbContext> dbContextProvider,
            IObjectMapper objectMapper) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _objectMapper = objectMapper;
        }

        public async Task AddRangeAsync(List<VehicleType> entityList, CancellationToken cancellationToken = default)
        {
            await base.DbContext.VehicleTypes.AddRangeAsync(entityList, cancellationToken);
        }

        public void UpdateRange(List<VehicleType> entityList)
        {
            base.DbContext.VehicleTypes.UpdateRange(entityList);
        }

    }
}