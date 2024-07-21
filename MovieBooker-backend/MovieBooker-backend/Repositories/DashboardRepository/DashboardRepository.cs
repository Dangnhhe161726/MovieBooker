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
        public List<ChartDashboardDTO> GetDashboardChartInfo(string chartType)
        {
            List<ChartDashboardDTO> dashboardInfo = new List<ChartDashboardDTO>();

            if (chartType == "monthly")
            {
                //Sales and Orders info of 8 months consecutively
                DateTime currentMonth = DateTime.Now;
                for (int i = 7; i >= 0; i--)
                {
                    DateTime monthDate = currentMonth.AddMonths(-i);
                    ChartDashboardDTO monthInfo = new ChartDashboardDTO
                    {
                        Time = monthDate.ToString("MMM"),
                        Orders = CountOrders(currentMonth.AddMonths(-i - 1), currentMonth.AddMonths(-i)),
                        Sales = CalculateTotalSales(currentMonth.AddMonths(-i - 1), currentMonth.AddMonths(-i))
                    };
                    dashboardInfo.Add(monthInfo);
                }
            }
            else if (chartType == "yearly")
            {
                DateTime currentYear = DateTime.Now;
                for (int i = 7; i >= 0; i--)
                {
                    DateTime year = currentYear.AddYears(-i);
                    ChartDashboardDTO yearInfo = new ChartDashboardDTO
                    {
                        Time = year.ToString("yyyy"),
                        Orders = CountOrders(currentYear.AddYears(-i - 1), currentYear.AddYears(-i)),
                        Sales = CalculateTotalSales(currentYear.AddYears(-i - 1), currentYear.AddYears(-i))
                    };
                    dashboardInfo.Add(yearInfo);
                }
            }
            else
            {
                DateTime currentWeek = DateTime.Now;
                for (int i = 7; i >= 0; i--)
                {
                    DateTime week = currentWeek.AddDays(-i*7);
                    ChartDashboardDTO weekInfo = new ChartDashboardDTO
                    {
                        Time = week.ToString("dd/MM"),
                        Orders = CountOrders(currentWeek.AddDays(-i * 7 - 7), currentWeek.AddDays(-i * 7)),
                        Sales = CalculateTotalSales(currentWeek.AddDays(-i * 7 - 7), currentWeek.AddDays(-i * 7))
                    };
                    dashboardInfo.Add(weekInfo);
                }
            }

            return dashboardInfo;
        }
        public List<MovieDashboardDTO> GetDashboardMovieInfo()
        {
            DateTime currentMonthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateTime currentDate = DateTime.Today;

            var movieList = _context.Revervations
                .Where(r => r.ReservationDate >= currentMonthStart && r.ReservationDate <= currentDate)
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
                        TotalOrders = CountOrders(currentMonthStart, currentDate, movie.MovieId),
                        TotalSales = CalculateTotalSales(currentMonthStart, currentDate, movie.MovieId),
                    };
                    dashboardInfo.Add(movieInfo);
                }
            }

            // Sort the dashboard information by TotalSales
            dashboardInfo = dashboardInfo.OrderByDescending(d => d.TotalSales).ToList();
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
