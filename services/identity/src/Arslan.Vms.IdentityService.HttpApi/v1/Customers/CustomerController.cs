using Arslan.Vms.IdentityService.v1.Customers.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.IdentityService.v1.Customers
{
	[ApiVersion("1.0")]
	[ApiController]
	[Area("Base")]
	[ControllerName("Customer")]
	[Route("identity/v{version:apiVersion}/base/customer")]
	public class IdentityCustomerController : IdentityServiceController, ICustomerAppService
	{
		#region Fields
		protected ICustomerAppService _customerAppService { get; }
		#endregion

		#region Ctor
		public IdentityCustomerController(ICustomerAppService customerAppService)
		{
			_customerAppService = customerAppService;
		}
		#endregion


		[HttpPost]
		public Task<CustomerDto> CreateAsync(CreateCustomerDto input)
		{
			ValidateModel();
			return _customerAppService.CreateAsync(input);
		}

		[HttpPut]
		[Route("{id}")]
		public virtual Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto input)
		{
			return _customerAppService.UpdateAsync(id, input);
		}

		[HttpDelete]
		public virtual Task DeleteAsync([FromForm(Name = "key")] string key)
		{
			return _customerAppService.DeleteAsync(key);
		}

		[HttpPost]
		[Route("Undo")]
		public virtual Task UndoAsync(Guid id)
		{
			return _customerAppService.UndoAsync(id);
		}

		[HttpPost]
		[Route("UndoVehicle")]
		public Task UndoVehcileAsync(Guid userId, Guid vehicleId)
		{
			return _customerAppService.UndoVehcileAsync(userId, vehicleId);
		}

		[HttpGet]
		[Route("{id}/{isDeleted}")]
		public virtual Task<CustomerDto> GetAsync(Guid id, bool isDeleted = false)
		{
			return _customerAppService.GetAsync(id, isDeleted);
		}

		[HttpGet]
		public virtual Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
		{
			return _customerAppService.GetListAsync(loadOptions);
		}

		[HttpGet]
		[Route("BySearchContent")]
		public virtual Task<List<CustomerSearchDto>> GetBySearchContent(string nameSurnameGsmPlateNo)
		{
			return _customerAppService.GetBySearchContent(nameSurnameGsmPlateNo);
		}

		[HttpGet]
		[Route("ByVehicleId/{userVehicleId}")]
		public virtual Task<CustomerVehicleSearchDto> GetByVehicleId(Guid userId, Guid vehicleId)
		{
			return _customerAppService.GetByVehicleId(userId, vehicleId);
		}

		[HttpGet]
		[Route("CustomerDetails")]
		public async Task<CustomerReportDto> GetCustomerDetails(Guid userId, Guid vehicleId)
		{
			return await _customerAppService.GetCustomerDetails(userId, vehicleId);
		}

		[HttpGet]
		[Route("CustomerVehicles")]
		public async Task<List<CustomerVehiclehDto>> GetCustomerVehicles(Guid customerId)
		{
			return await _customerAppService.GetCustomerVehicles(customerId);
		}

		[HttpGet]
		[Route("CustomerVehicle")]
		public async Task<CustomerVehiclehDto> GetCustomerVehicle(Guid userId, Guid vehicleId)
		{
			return await _customerAppService.GetCustomerVehicle(userId, vehicleId);
		}

		[HttpGet]
		[Route("All")]
		public async Task<List<CustomerListDto>> GetAllAsync()
		{
			return await _customerAppService.GetAllAsync();
		}

		[HttpGet]
		[Route("VehicleList")]
		public async Task<LoadResult> GetVehicleListAsync(DataSourceLoadOptions loadOptions)
		{
			return await _customerAppService.GetVehicleListAsync(loadOptions);
		}
	}
}