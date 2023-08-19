using System;
using Arslan.Vms.Orders.Reports;
using Arslan.Vms.OrderService.v1.Reports;
using DevExpress.XtraReports.UI;

namespace Arslan.Vms.Orders.Reports
{
    public partial class WorkorderXtraReport1
    {
        public WorkorderXtraReport1()
        {
            InitializeComponent();
        }

        public WorkorderXtraReport1(SalesOrderReport data)
        {
            InitializeComponent();
            objectDataSource1.DataSource = data;
        }
    }
}
