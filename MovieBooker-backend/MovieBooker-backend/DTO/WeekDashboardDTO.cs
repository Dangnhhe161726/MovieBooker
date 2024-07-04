namespace MovieBooker_backend.DTO
{
    public class WeekDashboardDTO
    {
        public int NumberOfUsers { get; set; }
        public int NumberOfOrders { get; set; }
        public double TotalSales { get; set; }
        public double OrdersChangePercentage { get; set; }
        public double SalesChangePercentage { get; set; }
    }
}
