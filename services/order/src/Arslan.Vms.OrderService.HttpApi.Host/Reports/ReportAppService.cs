using Arslan.Vms.Orders.Reports;
using Arslan.Vms.OrderService.Localization;
using Arslan.Vms.OrderService.Permissions;
using Arslan.Vms.OrderService.SalesOrders;
using Arslan.Vms.OrderService.v1.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.MultiTenancy;

namespace Arslan.Vms.OrderService.v1.Reports
{
    [Authorize(OrderServicePermissions.SalesOrder.Default)]
    public class ReportAppService : OrderServiceAppService, IReportAppService
    {
        private readonly IRepository<SalesOrder, Guid> _salesOrderRepository;
        //private readonly IRepository<Product, Guid> _productRepository;
        //private readonly IUserRepository _userRepository;
        //private readonly IRepository<Company, Guid> _companyRepository;
        private readonly ICurrentTenant _currentTenant;
        //private readonly ICustomerAppService _customerAppService;
        private readonly IFileAppService _fileAppService;
        private readonly IStringLocalizer<OrderServiceResource> _localizer;
        private readonly IDataFilter _dataFilter;

        public ReportAppService(
       //IRepository<Product, Guid> productRepository,
       IRepository<SalesOrder, Guid> salesOrderRepository,
       //IUserRepository userRepository,
       //IRepository<Company, Guid> companyRepository,
       ICurrentTenant currentTenant,
       //ICustomerAppService customerAppService,
       IFileAppService fileAppService,
       IDataFilter dataFilter,
       IStringLocalizer<OrderServiceResource> localizer
       )
        {
            //_productRepository = productRepository;
            _salesOrderRepository = salesOrderRepository;
            //_userRepository = userRepository;
            _currentTenant = currentTenant;
            //_customerAppService = customerAppService;
            //_companyRepository = companyRepository;
            _fileAppService = fileAppService;
            _dataFilter = dataFilter;
            _localizer = localizer;
        }
        [HttpGet]
        public async Task<SalesOrderReport> Generate(string workorderNo, string basePath)
        {
            using (_dataFilter.Disable<ISoftDelete>())
            {
                var salesorder = _salesOrderRepository.WithDetails().FirstOrDefault(f => f.OrderNumber == workorderNo);
                //var customer = await _customerAppService.GetCustomerDetails(salesorder.CustomerId, salesorder.VehicleId.Value);
                //var headWorker = await _userRepository.FirstOrDefaultAsync(w => w.Id == salesorder.HeadTechnicianId);
                //var company = await _companyRepository.FirstOrDefaultAsync(f => f.TenantId == _currentTenant.Id);

                //long.TryParse(customer.HomePhoneNumber, out long lognHomePhone);

                //long.TryParse(customer.WorkPhoneNumber, out long lognWorkPhone);

                //long.TryParse(customer.PhoneNumber, out long lognMobilePhone);

                //var report = new SalesOrderReport
                //{
                //    UserAddress = customer.Address,
                //    UserHomePhone = customer.HomePhoneNumber,
                //    UserWorkPhone = customer.WorkPhoneNumber,
                //    UserMobilePhone = customer.PhoneNumber,
                //    UserHomePhoneInt = lognMobilePhone,
                //    UserWorkPhoneInt = lognWorkPhone,
                //    UserMobilePhoneInt = lognMobilePhone,
                //    UserName = customer.UserCn,
                //    VehicleArrivedByVehicle = salesorder.VehicleReceiveFrom,//TODO::
                //    VehicleReceiveDate = salesorder.VehicleReceiveDate,
                //    VehicleChassis = customer.VehicleChassis,
                //    VehicleColor = customer.VehicleColor,
                //    VehicleEnteredKM = salesorder.Kilometrage,
                //    VehicleMotor = customer.VehicleMotor,
                //    VehiclePlateNo = customer.VehiclePlateNo,
                //    VehicleModelType = customer.VehicleBrandName,
                //    HeadWorker = headWorker?.Name + " " + headWorker?.Surname,
                //    Notes = salesorder.Notes,
                //    WorkorderNo = salesorder.OrderNumber,//TODO::
                //    LabelUserName = _localizer["Report:VehicleOwner"].Value,
                //    LabelGeneralTotalPrice = _localizer["Report:GeneralTotalPrice"].Value,
                //    LabelHeadWorker = _localizer["Report:HeadTechnitian"].Value,
                //    LabelNotes = _localizer["Report:Nots"].Value,
                //    LabelTaxRate = _localizer["Report:Tax"].Value,
                //    LabelToolTotalPrice = _localizer["Report:ProductTotalPrice"].Value,
                //    LabelUserAddress = _localizer["Report:Address"].Value,
                //    LabelUserHomePhone = _localizer["Report:HomePhone"].Value,
                //    LabelUserMobilePhone = _localizer["Report:GSM"].Value,
                //    LabelUserWorkPhone = _localizer["Report:WorkPhone"].Value,
                //    LabelVehicleArrivedByVehicle = _localizer["Report:VehicleArrivedBy"].Value,
                //    LabelVehicleChassis = _localizer["Report:VehicleChassis"].Value,
                //    LabelVehicleColor = _localizer["Report:VehicleColor"].Value,
                //    LabelVehicleEnteredKM = _localizer["Report:VehicleEnteredKM"].Value,
                //    LabelVehicleModelType = _localizer["Report:VehicleModelType"].Value,
                //    LabelVehicleMotor = _localizer["Report:VehicleMotor"].Value,
                //    LabelVehiclePlateNo = _localizer["Report:VehiclePlateNo"].Value,
                //    LabelVehicleReceiveDate = _localizer["Report:VehicleReceiveDate"].Value,
                //    LabelWorkTotalPrice = _localizer["Report:ServiceTotalPrice"].Value,
                //    LabelSignature = _localizer["Report:Signature"].Value,
                //    LabelSignatureDescription = _localizer["Report:SignatureDescription"].Value,

                //    LabelTaxRatio = _localizer["Report:VehicleOwner"].Value,
                //    LabelWorkorderNo = _localizer["Report:VehicleOwner"].Value,
                //};

                //if (company.LogoAttachmentId != null)
                //{
                //    var file = await _fileAppService.GetAsync(company.LogoAttachmentId.Value);
                //    var path = Path.Combine(basePath, file.Id.ToString());

                //    var memory = new MemoryStream();
                //    using (var stream = new FileStream(path, FileMode.Open))
                //    {
                //        await stream.CopyToAsync(memory);
                //    }
                //    memory.Position = 0;

                //    report.Logo = memory.ToArray();
                //}

                //var ss = salesorder.OrderLines;

                //var localizationItem = new SalesOrderReportItem
                //{
                //    LabelProductName = _localizer["Report:ProductName"].Value,
                //    LabelProductPrice = _localizer["Report:ProductPrice"].Value,
                //    LabelServiceName = _localizer["Report:ServiceName"].Value,
                //    LabelServicePrice = _localizer["Report:ServicePrice"].Value,
                //    LabelTechnician = _localizer["Report:ServiceTechnician"].Value,
                //    LabelTotalProductPrice = _localizer["Report:ProductTotalPrice"].Value,
                //    LabelTotalServicePrice = _localizer["Report:ServiceTotalPrice"].Value,
                //};

                //foreach (var item in ss)
                //{
                //    var product = await _productRepository.FirstOrDefaultAsync(w => w.Id == item.ProductId);
                //    //if (product.ProductType != ProductType.Service)
                //    //{
                //    //    report.Products.Add(new SalesOrderReportItem
                //    //    {
                //    //        Name = product.Name,
                //    //        Price = item.UnitPrice,
                //    //        LabelProductName = localizationItem.LabelProductName,
                //    //        LabelProductPrice = localizationItem.LabelProductPrice,
                //    //        LabelServiceName = localizationItem.LabelServiceName,
                //    //        LabelServicePrice = localizationItem.LabelServicePrice,
                //    //        LabelTechnician = localizationItem.Technician,
                //    //        LabelTotalProductPrice = localizationItem.LabelTotalProductPrice,
                //    //        LabelTotalServicePrice = localizationItem.LabelTotalServicePrice,
                //    //    });
                //    //}
                //    //if (product.ProductType == ProductType.Service)
                //    //{
                //    //    report.Services.Add(new SalesOrderReportItem
                //    //    {
                //    //        Name = product.Name,
                //    //        Price = item.UnitPrice,
                //    //        LabelProductName = localizationItem.LabelProductName,
                //    //        LabelProductPrice = localizationItem.LabelProductPrice,
                //    //        LabelServiceName = localizationItem.LabelServiceName,
                //    //        LabelServicePrice = localizationItem.LabelServicePrice,
                //    //        LabelTechnician = localizationItem.Technician,
                //    //        LabelTotalProductPrice = localizationItem.LabelTotalProductPrice,
                //    //        LabelTotalServicePrice = localizationItem.LabelTotalServicePrice,
                //    //    });
                //    //}
                //}

                //report.ToolTotalPrice = report.Products.Sum(s => s.Price);
                //report.WorkTotalPrice = report.Services.Sum(s => s.Price);
                //report.TaxRate = (report.ToolTotalPrice + report.WorkTotalPrice) * 18 / 100;
                //report.GeneralTotalPrice = report.ToolTotalPrice + report.WorkTotalPrice + report.TaxRate;

                //return await Task.FromResult(report);

                return null;
            }
        }

        [HttpGet]
        public async Task<MemoryStream> Download(string workorderNo, string baseUrl)
        {
            var stream = new MemoryStream();
            var report = new WorkorderXtraReport1(await Generate(workorderNo, baseUrl));

            report.ExportToPdf(stream);

            return stream;
        }
    }
}