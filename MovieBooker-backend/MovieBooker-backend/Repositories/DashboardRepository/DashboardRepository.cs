using MovieBooker_backend.DTO;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Data;

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
        public List<MonthDashboardDTO> GetDashboardMonthlyInfo()
        {
            List<MonthDashboardDTO> dashboardInfo = new List<MonthDashboardDTO>();

            //Sales and Orders info of 8 months consecutively
            DateTime currentMonth = DateTime.Now;
            for (int i = 7; i >= 0; i--)
            {
                MonthDashboardDTO monthInfo = new MonthDashboardDTO
                {
                    Month = currentMonth.AddMonths(-i).Month.ToString(),
                    Orders = CountOrders(currentMonth.AddMonths(-i - 1), currentMonth.AddMonths(-i)),
                    Sales = CalculateTotalSales(currentMonth.AddMonths(-i - 1), currentMonth.AddMonths(-i))
                };
                dashboardInfo.Add(monthInfo);
            }

            return dashboardInfo;
        }
        public List<MovieDashboardDTO> GetDashboardMovieInfo()
        {
            DateTime currentMonth = DateTime.Today;
            DateTime lastMonth = DateTime.Today.AddMonths(-1);
;
            var movieList = _context.Revervations
                .Where(r => r.ReservationDate >= lastMonth && r.ReservationDate <= currentMonth)
                .Select(r => r.Movie)
                .Distinct()
                .ToList();

            List<MovieDashboardDTO> dashboardInfo = new List<MovieDashboardDTO>();
            foreach (Movie? movie in movieList)
            {
                if (movie != null)
                {
                    MovieDashboardDTO movieInfo = new MovieDashboardDTO()
                    {
                        MovieTitle = movie.MovieTitle,
                        TotalOrders = CountOrders(lastMonth, currentMonth, movie.MovieId),
                        TotalSales = CalculateTotalSales(lastMonth, currentMonth, movie.MovieId),
                    };
                    dashboardInfo.Add(movieInfo);
                }
            }
            dashboardInfo.OrderBy(d => d.TotalSales);
            return dashboardInfo;
        }

        public int CountOrders(DateTime startTime, DateTime endTime)
        {
            int numberOfOrders = _context.Revervations
                                 .Where(r => r.ReservationDate >= startTime && r.ReservationDate <= endTime)
                                 .Count();
            return numberOfOrders;
        }
        public int CountOrders(DateTime startTime, DateTime endTime, int movieId)
        {
            int numberOfOrders = _context.Revervations
                                 .Where(r => r.MovieId == movieId && r.ReservationDate >= startTime && r.ReservationDate <= endTime)
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
        public double CalculateTotalSales(DateTime startTime, DateTime endTime, int movieId)
        {
            double totalOfSales = _context.Revervations
                                  .Where(r => r.MovieId == movieId && r.ReservationDate >= startTime && r.ReservationDate <= endTime && r.TotalAmount != null)
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
