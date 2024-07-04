using MovieBooker_backend.DTO;
using MovieBooker_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MovieBooker_backend.Repositories.DashboardRepository
{
    public interface IDashboardRepository
    {
        public double CalculateTotalSales(DateTime startTime, DateTime endTime);
        public int CountOrders(DateTime startTime, DateTime endTime);
        public int CountUsers();
        public WeekDashboardDTO GetDashboardWeeklyInfo();
        public List<MonthDashboardDTO> GetDashboardMonthlyInfo();
        public List<MovieDashboardDTO> GetDashboardMovieInfo();
    }
}
