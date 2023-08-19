using Arslan.Vms.OrderService.v1.Dtos;
using Arslan.Vms.OrderService.v1.PurchaseOrders.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arslan.Vms.OrderService.v1.PurchaseOrders
{
    [ApiController]
    //[RemoteService(Name = "ArslanVmsOrders")]
    [Area("Order")]
    [ControllerName("PurchaseOrder")]
    [Route("rest/api/latest/vms/orders/purchase")]
    //[ApiVersion("1.0")]
    public class PurchaseOrderController : OrderServiceController, IPurchaseOrderAppService
    {
        protected IPurchaseOrderAppService _purchaseOrderAppService { get; }

        public PurchaseOrderController(IPurchaseOrderAppService purchaseOrderAppService)
        {
            _purchaseOrderAppService = purchaseOrderAppService;
        }

        [HttpPost]
        public Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto input)
        {
            ValidateModel();
            return _purchaseOrderAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<PurchaseOrderDto> UpdateAsync(Guid id, UpdatePurchaseOrderDto input)
        {
            return _purchaseOrderAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public virtual Task DeleteAsync([FromForm(Name = "key")] string key)
        {
            return _purchaseOrderAppService.DeleteAsync(key);
        }

        [HttpPost]
        [Route("Undo")]
        public virtual Task UndoAsync(Guid id)
        {
            return _purchaseOrderAppService.UndoAsync(id);
        }

        [HttpGet]
        [Route("{id}/{isDeleted}")]
        public virtual Task<PurchaseOrderDto> GetAsync(Guid id, bool isDeleted = false)
        {
            return _purchaseOrderAppService.GetAsync(id, isDeleted);
        }

        [HttpGet]
        public virtual Task<LoadResult> GetListAsync(DataSourceLoadOptions loadOptions)
        {
            return _purchaseOrderAppService.GetListAsync(loadOptions);
        }

        [HttpGet]
        [Route("StatusList")]
        public Task<List<StatusDto>> GetStatusListAsync()
        {
            return _purchaseOrderAppService.GetStatusListAsync();
        }

        [HttpGet]
        [Route("InventoryStatusList")]
        public Task<List<StatusDto>> GetInventoryStatusListAsync()
        {
            return _purchaseOrderAppService.GetInventoryStatusListAsync();
        }

        [HttpGet]
        [Route("PaymentStatusList")]
        public Task<List<StatusDto>> GetPaymentStatusListAsync()
        {
            return _purchaseOrderAppService.GetPaymentStatusListAsync();
        }
    }
}