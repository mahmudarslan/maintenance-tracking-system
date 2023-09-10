using System;
using System.Collections.Generic;

namespace Arslan.Vms.OrderService.v1.Reports
{
    public class SalesOrderReport
    {
        public SalesOrderReport()
        {
            Products = new List<SalesOrderReportItem>();
            Services = new List<SalesOrderReportItem>();
        }

        public byte[] Logo { get; set; }
        public string UserName { get; set; }
        public string UserAddress { get; set; }
        public string UserHomePhone { get; set; }
        public string UserWorkPhone { get; set; }
        public string UserMobilePhone { get; set; }
        public long UserHomePhoneInt { get; set; }
        public long UserWorkPhoneInt { get; set; }
        public long UserMobilePhoneInt { get; set; }
        public string VehiclePlateNo { get; set; }
        public DateTime? VehicleReceiveDate { get; set; }
        public int VehicleEnteredKM { get; set; }
        public string VehicleArrivedByVehicle { get; set; }
        public string VehicleModelType { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleChassis { get; set; }
        public string VehicleMotor { get; set; }
        public string Notes { get; set; }
        public string HeadWorker { get; set; }
        public string WorkorderNo { get; set; }
        public decimal TaxRate { get; set; }
        public string TaxRatio { get; set; }
        public decimal WorkTotalPrice { get; set; }
        public decimal ToolTotalPrice { get; set; }
        public decimal GeneralTotalPrice { get; set; }
        public string LabelUserName { get; set; }
        public string LabelUserAddress { get; set; }
        public string LabelUserHomePhone { get; set; }
        public string LabelUserWorkPhone { get; set; }
        public string LabelUserMobilePhone { get; set; }
        public string LabelVehiclePlateNo { get; set; }
        public string LabelVehicleReceiveDate { get; set; }
        public string LabelVehicleEnteredKM { get; set; }
        public string LabelVehicleArrivedByVehicle { get; set; }
        public string LabelVehicleModelType { get; set; }
        public string LabelVehicleColor { get; set; }
        public string LabelVehicleChassis { get; set; }
        public string LabelVehicleMotor { get; set; }
        public string LabelNotes { get; set; }
        public string LabelHeadWorker { get; set; }
        public string LabelWorkorderNo { get; set; }
        public string LabelTaxRate { get; set; }
        public string LabelTaxRatio { get; set; }
        public string LabelWorkTotalPrice { get; set; }
        public string LabelToolTotalPrice { get; set; }
        public string LabelGeneralTotalPrice { get; set; }
        public string LabelSignature { get; set; }
        public string LabelSignatureDescription { get; set; }

        public List<SalesOrderReportItem> Products { get; set; }
        public List<SalesOrderReportItem> Services { get; set; }

    }

    public class SalesOrderReportItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Technician { get; set; }
        public string LabelProductName { get; set; }
        public string LabelProductPrice { get; set; }
        public string LabelTotalProductPrice { get; set; }
        public string LabelTechnician { get; set; }
        public string LabelServiceName { get; set; }
        public string LabelServicePrice { get; set; }
        public string LabelTotalServicePrice { get; set; }
    }
}