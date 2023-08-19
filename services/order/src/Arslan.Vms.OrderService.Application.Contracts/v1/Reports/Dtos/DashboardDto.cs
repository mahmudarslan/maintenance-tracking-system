namespace Arslan.Vms.OrderService.v1.Reports.Dtos
{
    public class DashboardDto
    {
        public int Quote { get; set; }
        public int Open { get; set; }
        public int InProgress { get; set; }
        public int Invoiced { get; set; }
        public int Paid { get; set; }
        public int Cancelled { get; set; }
        public int WaitingForApprove { get; set; }
    }
}