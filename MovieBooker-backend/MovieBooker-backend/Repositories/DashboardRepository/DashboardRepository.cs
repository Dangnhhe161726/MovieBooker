using MovieBooker_backend.DTO;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MovieBooker_backend.Repositories.DashboardRepository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly bookMovieContext _context;

        public DashboardRepository(bookMovieContext context)
        {
            _context = context;
        }

        public WeekDashboardDTO GetDashboardWeeklyInfo()
        {
            DateTime now = DateTime.Now;
            DateTime oneWeekAgo = now.AddDays(-7);

            int currentNumberOfOrders = CountOrders(oneWeekAgo, now);
            int previousNumberOfOrders = CountOrders(oneWeekAgo.AddDays(-7), oneWeekAgo);

            double currentTotalSales = CalculateTotalSales(oneWeekAgo, now);
            double previousTotalSales = CalculateTotalSales(oneWeekAgo.AddDays(-7), oneWeekAgo);

            WeekDashboardDTO dashboardInfo = new WeekDashboardDTO()
            {
                NumberOfOrders = currentNumberOfOrders,
                NumberOfUsers = CountUsers(),
                TotalSales = currentTotalSales,

                OrdersChangePercentage = CalculatePercentageChange(previousNumberOfOrders, currentNumberOfOrders),
                SalesChangePercentage = CalculatePercentageChange(previousTotalSales, currentTotalSales)
            };

            return dashboardInfo;
        }
        public MonthDashboardDTO GetDashboardMonthlyInfo()
        {
            return new MonthDashboardDTO();
        }

        public int CountOrders(DateTime startTime, DateTime endTime)
        {
            int numberOfOrders = _context.Revervations
                                 .Where(r => r.ReservationDate >= startTime && r.ReservationDate <= endTime)
                                 .Count();
            return numberOfOrders;
        }

        public double CalculateTotalSales(DateTime startTime, DateTime endTime)
        {
            double totalOfSales = _context.Revervations
                                  .Where(r => r.ReservationDate >= startTime && r.ReservationDate <= endTime && r.TotalAmount != null)
                                  .Sum(r => r.TotalAmount.Value);
            return totalOfSales;
        }

        public int CountUsers()
        {
            int numberOfUsers = _context.Users
                                .Where(u => u.Status != null && u.Status.Value == true) //Active User
                                .Count();
            return numberOfUsers;
        }

        public double CalculatePercentageChange(double previousValue, double currentValue) 
        {
            if (previousValue == 0)
            {
                return currentValue > 0 ? 100 : 0;
            }
            return ((currentValue - previousValue) / previousValue) * 100;
        }
    }
}
