using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace Arslan.Vms.ProductService.Locations
{
    public class LocationDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Location, Guid> _locationRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly ICurrentTenant _currentTenant;

        public LocationDataSeedContributor(
            IRepository<Location, Guid> locationRepository,
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator)
        {
            _locationRepository = locationRepository;
            _guidGenerator = guidGenerator;
            _currentTenant = currentTenant;
        }

        [UnitOfWork]
        public virtual async Task SeedAsync(DataSeedContext context)
        {
            await CreateLocation();
        }

        async Task CreateLocation()
        {
            var locations = _locationRepository.GetListAsync().Result;

            if (!locations.Select(s => s.Name).Contains("Default Location"))
            {
                var location = new Location(_guidGenerator.Create(), _currentTenant.Id, null, "Default Location", 1);
                locations.Add(location);
                await _locationRepository.InsertAsync(location);
            }
        }
    }
}