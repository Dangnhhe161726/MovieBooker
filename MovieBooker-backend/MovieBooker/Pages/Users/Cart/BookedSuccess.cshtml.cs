using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using MovieBooker.DTO;
using MovieBooker.Models;
using Newtonsoft.Json;

namespace MovieBooker.Pages.Users.Cart
{
    public class BookedSuccessModel : PageModel
    {
        public List<UserDTO> Users { get; set; }
        public List<SeatDTO> seats { get; set; }
        public List<ScheduleDTO> schedules { get; set; }
        public async Task OnGetAsync()
        {
            HttpClient _httpClient = new HttpClient();
            var reservationjson = TempData["resevation"].ToString();
            string emails =(string) TempData["confirmemail"];
            int scheduleId =(int) TempData["scheduleId"];

            var seatIdJson = TempData["seatId"].ToString();
            List<int> seatId = JsonConvert.DeserializeObject<List<int>>(seatIdJson);
            List<CreateResevationDTO> reservation = JsonConvert.DeserializeObject<List<CreateResevationDTO>>(reservationjson);
            foreach (var item in reservation)
            {
                HttpResponseMessage response = await
               _httpClient.PostAsJsonAsync("https://localhost:5000/api/Reservation/CreateReservation", item);
            }
            HttpResponseMessage response2 = await _httpClient.GetAsync($"https://localhost:5000/api/User/GetAllUser?$filter=email eq '{emails}'");
            if (response2.IsSuccessStatusCode)
            {
                Users = await response2.Content.ReadFromJsonAsync<List<UserDTO>>();
            }
            HttpResponseMessage response3 = await _httpClient.GetAsync($"https://localhost:5000/api/Seat/GetSeat");
            if (response3.IsSuccessStatusCode)
            {
                var allSeats = await response3.Content.ReadFromJsonAsync<List<SeatDTO>>();
                seats = allSeats.Where(seat => seatId.Contains(seat.SeatId)).ToList();
            }
            HttpResponseMessage response4 = await
             _httpClient.GetAsync($"https://localhost:5000/api/Schedule/GetSchedule?$filter=schedulesId eq {scheduleId}");
            if (response4.IsSuccessStatusCode)
            {
                schedules = await response4.Content.ReadFromJsonAsync<List<ScheduleDTO>>();
            }
 
            string userName =  Users.FirstOrDefault()?.UserName ?? "";
            string movieName = schedules.FirstOrDefault()?.MovieTitle ?? "";
            string timeslot = schedules.FirstOrDefault()?.TimeSlot ?? "";
            string ScheduleDate = schedules.FirstOrDefault()?.ScheduleDate.Value.ToString("dd-MM-yyyy") ?? "";
            string TheaterName = schedules.FirstOrDefault()?.TheaterName ?? "";
            int numberticket = reservation.Count();
            double price = (double)reservation.First().TotalAmount;
            string seat = string.Join(", ", seats.Select(s => s.SeatNumber));
            User User = new User();
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(User.From));
            email.To.Add(MailboxAddress.Parse(emails));
            email.Subject = "Đặt Vé Thành Công";
            string emailBody = $@"
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f5f5f5;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}
        h2 {{
            color: #da0f32;
            text-align: center;
            margin-bottom: 20px;
        }}
        .movie-info {{
            background-color: #f0f0f0;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
        }}
        .ticket-info {{
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
        }}
        .footer {{
            text-align: center;
            margin-top: 20px;
            color: #666666;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header-image'>
            <img src='https://res.cloudinary.com/dwfo16yhs/image/upload/v1720365551/img_prn231/zmnnjnulyrtvn6icfyf1.jpg' alt='Email Header Image' style='max-width: 100%; height: auto;'>
        </div>
        <h2>Xác nhận đặt vé xem phim thành công</h2>
        
        <div class='movie-info'>
            <h3>Thông tin phim:</h3>
            <p><strong>Tên phim:</strong> {movieName}</p>
            <p><strong>Thời gian chiếu:</strong> {ScheduleDate} {timeslot}</p>
            <p><strong>Rạp:</strong> {TheaterName}</p>
        </div>
        
        <div class='ticket-info'>
            <h3>Thông tin vé</h3>
            <p><strong>Ghế ngồi:</strong> {seat}</p>
            <p><strong>Giá:</strong> {numberticket} x {price} VND</p>
        </div>
        
        <div class='customer-info'>
            <h3>Thông tin người đặt</h3>
            <p><strong>Tên người đặt:</strong> {userName}</p>
        </div>
        
        <p>Vé của bạn đã được đặt thành công. Xin vui lòng đến trước giờ chiếu để nhận vé và tham gia buổi chiếu.</p>
        
    </div>
    <p class='footer'>CGV Cinema - Địa chỉ: 123 FPT University - Điện thoại: 0123 456 789</p>
</body>
</html>";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = emailBody };
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(User.From, User.PasswordSendMail);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
