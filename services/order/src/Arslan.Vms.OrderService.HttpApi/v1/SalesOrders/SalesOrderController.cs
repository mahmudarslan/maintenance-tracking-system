using Arslan.Vms.OrderService.v1.Dtos;
using Arslan.Vms.OrderService.v1.Files;
using Arslan.Vms.OrderService.v1.Reports;
using Arslan.Vms.OrderService.v1.SalesOrders.Dtos;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Arslan.Vms.OrderService.v1.SalesOrders
{
    [ApiController]
    //[RemoteService(Name = "ArslanVmsOrders")]
    [Area("Orders")]
    [ControllerName("SalesOrder")]
    [Route("rest/api/latest/vms/orders/sales")]
    //[ApiVersion("1.0")]
    public class SalesOrderController : OrderServiceController, ISalesOrderAppService
    {
        protected ISalesOrderAppService _salesOrderAppService { get; }
        protected IReportAppService _salesReportAppService { get; }
        private IWebHostEnvironment _hostingEnvironment;
        private FileController _fileController { get; }

        public SalesOrderController(ISalesOrderAppService salesOrderAppService,
            IReportAppService salesReportAppService,
            IWebHostEnvironment environment,
            FileController fileController)
        {
            _salesOrderAppService = salesOrderAppService;
            _salesReportAppService = salesReportAppService;
            _hostingEnvironment = environment;
            _fileController = fileController;
        }

        [HttpPost]
        public Task<SalesOrderDto> CreateAsync(CreateSalesOrderDto input)
        {
            ValidateModel();
            return _salesOrderAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<SalesOrderDto> UpdateAsync(Guid id, UpdateSalesOrderDto input)
        {
            return _salesOrderAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        public virtual Task DeleteAsync([FromForm(Name = "key")] string key)
        {
            return _salesOrderAppService.DeleteAsync(key);
        }

        [HttpPost]
        [Route("Undo")]
        public virtual Task UndoAsync(Guid id)
        {
            return _salesOrderAppService.UndoAsync(id);
        }

        [HttpGet]
        [Route("{id}/{isDeleted}")]
        public virtual Task<SalesOrderDto> GetAsync(Guid id, bool isDeleted = false)
        {
            return _salesOrderAppService.GetAsync(id, isDeleted);
        }

        [HttpGet]
        public virtual Task<LoadResult> GetListAsync([FromQuery] DataSourceLoadOptions input)
        {
            return _salesOrderAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("StatusList")]
        public Task<List<StatusDto>> GetStatusListAsync()
        {
            return _salesOrderAppService.GetStatusListAsync();
        }

        [HttpGet]
        [Route("WorkorderTypes")]
        public Task<List<WorkStatusDto>> GetWorkorderTypesListAsync()
        {
            return _salesOrderAppService.GetWorkorderTypesListAsync();
        }

        [HttpGet]
        [Route("InventoryStatusList")]
        public Task<List<StatusDto>> GetInventoryStatusListAsync()
        {
            return _salesOrderAppService.GetInventoryStatusListAsync();
        }

        [HttpGet]
        [Route("PaymentStatusList")]
        public Task<List<StatusDto>> GetPaymentStatusListAsync()
        {
            return _salesOrderAppService.GetPaymentStatusListAsync();
        }

        [HttpGet]
        [Route("Download")]
        public async Task<IActionResult> Download(string workorderNo)
        {
            var stream = await _salesReportAppService.Download(workorderNo, _fileController._uploadFullPath);

            stream.Seek(0, SeekOrigin.Begin);
            string mimeType = "application/pdf";

            return new FileStreamResult(stream, mimeType)
            {
                FileDownloadName = $"{workorderNo}.pdf",
                EnableRangeProcessing = true
            };
        }
    }
}