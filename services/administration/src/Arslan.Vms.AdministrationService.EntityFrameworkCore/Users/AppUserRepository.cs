using Arslan.Vms.AdministrationService.Addresses;
using Arslan.Vms.AdministrationService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.AdministrationService.Users
{
    public class AppUserRepository : EfCoreRepository<AdministrationServiceDbContext, User, Guid>, IUserRepository
    {
        private readonly IObjectMapper _objectMapper;
        IDbContextProvider<AdministrationServiceDbContext> _dbContextProvider;

        public AppUserRepository(IDbContextProvider<AdministrationServiceDbContext> dbContextProvider,
            IObjectMapper objectMapper) : base(dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
            _objectMapper = objectMapper;
        }

        public virtual async Task<List<Address>> GetAddresses(Guid userId)
        {
            var data = from a in DbContext.Set<UserAddress>()
                        join b in DbContext.Set<Address>() on a.AddressId equals b.Id
                        where a.UserId == userId
                        select b;

            return await data.ToListAsync();
        }

        //public virtual async Task<List<Vehicle>> GetVehicles(Guid userId)
        //{
        //    var data = from a in DbContext.Set<UserVehicle>()
        //                join b in DbContext.Set<Vehicle>() on a.VehicleId equals b.Id
        //                where a.UserId == userId
        //                select b;

        //    return await data.ToListAsync();
        //}


        //public virtual async Task UndoVehicleAsync(Guid userId, Guid vehicleId)
        //{
        //    var data = (from a in DbContext.Set<UserVehicle>()
        //                where a.UserId == userId && a.VehicleId == vehicleId
        //                select a).FirstOrDefault();
        //    data.IsDeleted = false;

        //    await DbContext.SaveChangesAsync();
        //}



        //public override IQueryable<AppUser> WithDetails()
        //{
        //    return base.WithDetails(w => w.UserVehicles);
        //}
    }
}