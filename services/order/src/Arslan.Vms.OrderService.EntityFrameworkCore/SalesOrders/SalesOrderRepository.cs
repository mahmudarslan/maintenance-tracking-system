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

namespace Arslan.Vms.OrderService.SalesOrders
{
    public class SalesOrderRepository : EfCoreRepository<IOrderServiceDbContext, SalesOrder, Guid>, ISalesOrderRepository
    {
        //private readonly IRepository<Product, Guid> _productRepository;
        //private readonly IRepository<User, Guid> _userRepository;
        private readonly IDataFilter _dataFilter;

        public SalesOrderRepository(IDbContextProvider<IOrderServiceDbContext> dbContextProvider,
            //IRepository<User, Guid> userRepository,
            //IRepository<Product, Guid> productRepository,
            IDataFilter dataFilter) : base(dbContextProvider)
        {
            //_productRepository = productRepository;
            //_userRepository = userRepository;
            _dataFilter = dataFilter;
        }

        public override IQueryable<SalesOrder> WithDetails()
        {
            return base.WithDetails(w => w.OrderLines, a => a.PickLines);
        }

        public async Task<SalesOrder> GetWithDeatilsAsync(Guid id)
        {
            var so = WithDetails().AsNoTracking().Where(w => w.Id == id).FirstOrDefault();

            if (so == null)
            {
                return null;
            }

            using (_dataFilter.Disable<ISoftDelete>())
            {
                //foreach (var line in so.PickLines)
                //{
                //    line.Product = await _productRepository.FirstOrDefaultAsync(f => f.Id == line.ProductId);
                //}
            }

            return await Task.FromResult(so);
        }


        //public async Task<SalesOrder> GetWithDeatils1Async(Guid id)
        //{
        //    var query = (
        //                    from so in _salesOrderRepository
        //                    from user in _userRepository.Where(w => w.Id == so.CustomerId)
        //                    from uv in user.UserVehicles.Where(w => w.Id == so.UserVehicleId)
        //                    from vehicleModel in _vehicleTypeRepository.Where(w => w.Id == uv.Vehicle.ModelId)
        //                    from vehicleBrand in _vehicleTypeRepository.Where(w => w.Id == vehicleModel.ParentId)

        //                    select new SalesOrderSearchDto
        //                    {
        //                        CreationTime = so.CreationTime,
        //                        Id = so.Id,
        //                        VehicleReceiveDate = so.VehicleReceiveDate,
        //                        OrderNumber = so.OrderNumber,
        //                        OrderDate = so.OrderDate,
        //                        Description = so.Description,
        //                        Name = user.Name + " " + user.Surname,
        //                        PhoneNumber = user.PhoneNumber,
        //                        Surname = user.Surname,
        //                        VehicleModelId = vehicleModel.Id,
        //                        VehicleBrandId = vehicleModel.ParentId.Value,
        //                        ModelName = vehicleModel.Name,
        //                        BrandName = vehicleBrand.Name,
        //                        PlateNo = uv.Vehicle.Plate,
        //                        InventoryStatus = so.InventoryStatus,
        //                        PaymentStatus = so.PaymentStatus,
        //                        LocationId = so.LocationId,
        //                        Total = so.Total,
        //                        AmountPaid = so.AmountPaid,
        //                        Balance = so.Balance,
        //                        IsDeleted = so.IsDeleted,
        //                        Version = so.Version
        //                    });

        //    return await Task.FromResult(so);
        //}
    }
}