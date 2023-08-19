using Arslan.Vms.AdministrationService.Users;
using Arslan.Vms.VehicleService.Localization;
using Arslan.Vms.VehicleService.Permissions;
using Arslan.Vms.VehicleService.Users;
using Arslan.Vms.VehicleService.v1.Vehicles.Dtos;
using Arslan.Vms.VehicleService.Vehicles;
using Arslan.Vms.VehicleService.Vehicles.VehicleTypes;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.VehicleService.v1.Vehicles
{
    [Authorize(VehicleServicePermissions.Vehicle.Default)]
    public class VehicleAppService : VehicleServiceAppService, IVehicleAppService
    {
        private readonly IRepository<VehicleType, Guid> _vehicleTypeRepository;
        private readonly IRepository<Vehicle, Guid> _vehicleRepository;
        //private readonly IRepository<UserVehicle, Guid> _userVehicleRepository;
        IUserRepository _appUserRepository;
        private readonly IStringLocalizer<VehicleServiceResource> _localizer;
        private readonly ICurrentTenant _currentTenant;
        private readonly IDataFilter _dataFilter;
        private readonly IGuidGenerator _guidGenerator;

        public VehicleAppService(
            IRepository<VehicleType, Guid> vehicleTypeRepository,
            IRepository<Vehicle, Guid> vehicleRepository,
            //IRepository<UserVehicle, Guid> userVehicleRepository,
            IUserRepository appUserRepository,
            IStringLocalizer<VehicleServiceResource> localizer,
            IGuidGenerator guidGenerator,
            IDataFilter dataFilter,
            ICurrentTenant currentTenant)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
            //_userVehicleRepository = userVehicleRepository;
            _vehicleRepository = vehicleRepository;
            _appUserRepository = appUserRepository;
            _localizer = localizer;
            _currentTenant = currentTenant;
            _dataFilter = dataFilter;
            _guidGenerator = guidGenerator;
        }

        [Authorize(VehicleServicePermissions.Vehicle.Create)]
        public async Task<VehicleDto> CreateAsync(CreateVehicleDto input)
        {
            //aynı plaka eklenemez
            //aynı şase eklenemez
            //var dbvehicleData = _vehicleRepository.FirstOrDefault(f => f.Plate == input.Plate || f.Chassis == input.Chassis);

            //if (dbvehicleData != null)
            //{
            //    throw new UserFriendlyException(_localizer["Error:PlateOrChassisShouldBeUnique"].Value + $"  --{input.Plate}--{input.Chassis}");
            //}
            var appUser = await _appUserRepository.GetAsync(input.UserlId);

            var vehicle = new Vehicle(_guidGenerator.Create(), _currentTenant.Id, input.Plate, input.Color, input.Motor, input.Chassis, input.ModelId);
            //appUser.AddVehicle(vehicle.Id);
            await _vehicleRepository.InsertAsync(vehicle, true);

            //var userVehilce = new UserVehicle(_guidGenerator.Create(), _currentTenant.Id, input.CustomerId, vehicle.Id);
            //await _userVehicleRepository.InsertAsync(userVehilce, true);

            await _appUserRepository.UpdateAsync(appUser, true);

            return ObjectMapper.Map<Vehicle, VehicleDto>(vehicle);
        }

        [Authorize(VehicleServicePermissions.Vehicle.Update)]
        public async Task<VehicleDto> UpdateAsync(Guid userId, UpdateVehicleDto input)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                //var dbUserVehicleData = _userVehicleRepository.FirstOrDefault(f => f.Id == input.UserVehicleId);

                var dbData = await _appUserRepository.GetAsync(userId);
                //var uv = dbData.UserVehicles.FirstOrDefault(f => f.VehicleId == input.Id);

                //aynı plaka eklenemez
                //aynı şase eklenemez
                //var dbvehicleData1 = dbvehicleData.FirstOrDefault(f => (f.Plate == input.Plate || f.Chassis == input.Chassis) && f.Id != dbUserVehicleData.VehicleId);

                //if (dbvehicleData1 != null)
                //{
                //    throw new UserFriendlyException(_localizer["Error:PlateOrChassisShouldBeUnique"].Value + $"  --{input.Plate}--{input.Chassis}");
                //}
                //var updateUserVehicle = (await _appUserRepository.GetVehicles(userId)).FirstOrDefault(f => f.Id == input.Id);
                ////var dbVehicleData = _vehicleRepository.FirstOrDefault(f => f.Id == dbUserVehicleData.VehicleId);
                //updateUserVehicle.SetModelId(input.ModelId);
                //updateUserVehicle.SetChassis(input.Chassis);
                //updateUserVehicle.SetColor(input.Color);
                //updateUserVehicle.SetMotor(input.Motor);
                //updateUserVehicle.SetPlate(input.Plate);
                //await _vehicleRepository.UpdateAsync(dbVehicleData, true);

                //if (uv.AppUserId != userId)
                //{
                //    uv.IsDeleted = true;
                //    //await _userVehicleRepository.UpdateAsync(dbUserVehicleData, true);

                //    //var userVehilce = new UserVehicle(_guidGenerator.Create(), _currentTenant.Id, input.CustomerId, dbVehicleData.Id);
                //    //await _userVehicleRepository.InsertAsync(userVehilce, true);
                //}
                //else
                //{
                //    uv.IsDeleted = false;
                //    //await _userVehicleRepository.UpdateAsync(dbUserVehicleData, true);
                //}

                await _appUserRepository.UpdateAsync(dbData, true);

                //return ObjectMapper.Map<Vehicle, VehicleDto>(updateUserVehicle);
                return new VehicleDto();
            }
        }

        [Authorize(VehicleServicePermissions.Vehicle.Delete)]
        public async Task DeleteAsync(Guid userId, Guid vehicleId)
        {
            var dbData = await _appUserRepository.FirstOrDefaultAsync(f => f.Id == userId);

            var vehicle = await _vehicleRepository.FirstOrDefaultAsync(f => f.Id == vehicleId);
            await _vehicleRepository.DeleteAsync(vehicle);

            //var userVehicles

            //var dbvehicleData = dbData.Vehicles.FirstOrDefault(f => f.VehicleId == vehicleId);
            //dbvehicleData.IsDeleted = true;
            //await _appUserRepository.UpdateAsync(dbData, true);
            //var dbUserVehicleData = await _userVehicleRepository.GetAsync(id);
            //await _userVehicleRepository.DeleteAsync(dbUserVehicleData, true);
        }

        [Authorize(VehicleServicePermissions.Vehicle.Undo)]
        public async Task UndoAsync(Guid userId, Guid vehicleId)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var dbData = await _appUserRepository.FirstOrDefaultAsync(f => f.Id == userId);

                //aynı araçtan ikitane aynı anda aktif olamaz
                //var dbvehicleData = _userVehicleRepository.FirstOrDefault(f => f.IsDeleted == false && f.VehicleId == dbData.VehicleId);
                //var dbvehicleData = dbData.Vehicles.FirstOrDefault(f => f.VehicleId == vehicleId);
                var vehicle = await _vehicleRepository.FirstOrDefaultAsync(f => f.Id == vehicleId);
                //if (dbvehicleData != null)
                //{
                //    throw new UserFriendlyException(_localizer["Error:VehicleSameTimeActive"].Value);
                //}

                vehicle.IsDeleted = false;

                await _appUserRepository.UpdateAsync(dbData, true);
            }
        }

        public async Task<VehicleDto> GetAsync(Guid id, bool isDeleted)
        {
            using (isDeleted == true ? _dataFilter.Disable<ISoftDelete>() : _dataFilter.Enable<ISoftDelete>())
            {
                var appUserRepository = await _appUserRepository.GetQueryableAsync();
                var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();
                var vehicleRepository = await _vehicleRepository.GetQueryableAsync();
                //var uservVhicleRepository = await _userVehicleRepository.GetQueryableAsync();

                return (from u in appUserRepository
                        //from uv in u.Vehicles
                        //from v in vehicleRepository.Where(w => w.Id == uv.VehicleId)
                        //from model in vehicleTypeRepository.Where(w => w.Id == v.ModelId)
                            //from brand in _vehicleTypeRepository.Where(w => w.Id == model.ParentId)
                        select new VehicleDto
                        {
                            //Id = v.Id,
                            //UserId = u.Id,
                            //BrandId = model.ParentId.Value,
                            //ModelId = model.Id,
                            ////BrandName = brand.Name,
                            ////ModelName = model.Name,
                            ////UserVehicleId = uv.Id,
                            ////UserCN = u.Name + " " + u.Surname,
                            //Chassis = v.Chassis,
                            //Color = v.Color,
                            //Motor = v.Motor,
                            //Plate = v.Plate,
                            //CreationTime = v.CreationTime,
                            //IsDeleted = uv.IsDeleted
                        }).FirstOrDefault();
            }
        }

        [Authorize(VehicleServicePermissions.Vehicle.List)]
        public async Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            loadOptions.PrimaryKey = new[] { "Id" };

            var appUserRepository = await _appUserRepository.GetQueryableAsync();
            var vehicleTypeRepository = await _vehicleTypeRepository.GetQueryableAsync();
            var vehicleRepository = await _vehicleRepository.GetQueryableAsync();
            //var uservVhicleRepository = await _userVehicleRepository.GetQueryableAsync();

            using (_dataFilter.Disable<ISoftDelete>())
            {
                var ls = from u in appUserRepository
                          //from uv in u.Vehicles
                          //from v in vehicleRepository.Where(w => w.Id == uv.VehicleId)
                          //from model in vehicleTypeRepository.Where(w => w.Id == v.ModelId)
                          //from brand in vehicleTypeRepository.Where(w => w.Id == model.ParentId)
                          select new VehicleListDto
                          {
                              //Id = v.Id,
                              //UserId = u.Id,
                              //BrandId = brand.Id,
                              //ModelId = model.Id,
                              //BrandName = brand.Name,
                              //ModelName = model.Name,
                              ////UserVehicleId = uv.Id,
                              //UserCN = u.Name + " " + u.Surname,
                              //Chassis = v.Chassis,
                              //Color = v.Color,
                              //Motor = v.Motor,
                              //Plate = v.Plate,
                              //CreationTime = v.CreationTime,
                              //IsDeleted = uv.IsDeleted
                          };

                return await DataSourceLoader.LoadAsync(ls, loadOptions);
            }
        }

    }
}