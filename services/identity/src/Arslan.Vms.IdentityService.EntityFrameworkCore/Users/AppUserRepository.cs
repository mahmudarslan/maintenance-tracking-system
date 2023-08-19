using Arslan.Vms.IdentityService.Addresses;
using Arslan.Vms.IdentityService.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.ObjectMapping;

namespace Arslan.Vms.IdentityService.Users
{
    public class AppUserRepository : EfCoreRepository<IdentityServiceDbContext, User, Guid>, IUserRepository
    {
        private readonly IObjectMapper _objectMapper;
        IDbContextProvider<IdentityServiceDbContext> _dbContextProvider;

        public AppUserRepository(IDbContextProvider<IdentityServiceDbContext> dbContextProvider,
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