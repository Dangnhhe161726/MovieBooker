using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.DashboardRepository;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

/*        [HttpGet("GetSales")]
        public IActionResult GetSales(DateTime startTime, DateTime endTime)
        {
            var totalSales = _dashboardRepository.CalculateTotalSales(startTime, endTime);
            return Ok(totalSales);
        }

        [HttpGet("GetOrders")]
        public IActionResult GetOrders(DateTime startTime, DateTime endTime)
        {
            var numOrders = _dashboardRepository.CountOrders(startTime, endTime);
            return Ok(numOrders);
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var numUsers = _dashboardRepository.CountUsers();
            return Ok(numUsers);
        }*/

        [HttpGet("GetWeeklyDashboard")]
        public IActionResult GetWeekDashboard()
        {
            var weekDashboard = _dashboardRepository.GetDashboardWeeklyInfo();
            return Ok(weekDashboard);
        }
    }
}
