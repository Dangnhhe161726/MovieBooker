using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Repositories.DashboardRepository;
using MovieBooker_backend.Repositories.ReservationRepository;
using OfficeOpenXml;

namespace MovieBooker_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IReservationRepository _reservationRepository;
        public DashboardController(IDashboardRepository dashboardRepository, IReservationRepository reservationRepository)
        {
            _dashboardRepository = dashboardRepository;
            _reservationRepository = reservationRepository;
        }

        [HttpGet("GetWeeklyDashboard")]
        public IActionResult GetWeekDashboard()
        {
            var weekDashboard = _dashboardRepository.GetDashboardWeeklyInfo();
            return Ok(weekDashboard);
        }

        [HttpGet("GetMonthlyDashboard")]
        public IActionResult GetMonthDashboard()
        {
            var monthDashboard = _dashboardRepository.GetDashboardMonthlyInfo();
            return Ok(monthDashboard);
        }

        [HttpGet("GetMovieDashboard")]
        public IActionResult GetMovieDashboard()
        {
            var movieDashboard = _dashboardRepository.GetDashboardMovieInfo();
            return Ok(movieDashboard);
        }

        [HttpGet("ExportReport")]
        public IActionResult ExportReport(string type)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var reportData = _reservationRepository.GenerateReport(type);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Report");
                worksheet.Cells["A1"].LoadFromDataTable(reportData, true);

                using (var range = worksheet.Cells["A1:D1"])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                };

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);

                stream.Position = 0;
                var fileName = $"{type}-report.xlsx";
                var mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(stream, mimeType, fileName);
            }
        }
    }
}
