using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using MovieBooker.Models;
using StackExchange.Redis;
using System.Net;
using MailKit.Net.Smtp;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MovieBooker.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConnectionMultiplexer _redisConnection;
        private readonly HttpClient _httpClient;

        public RegisterModel(IHttpClientFactory httpClientFactory, IConnectionMultiplexer redisConnection, HttpClient httpClient)
        {
            _httpClientFactory = httpClientFactory;
            _redisConnection = redisConnection;
            _httpClient = httpClient;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (User.Password != User.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Confirm Password incorrect.");
                return Page();
            }
            var httpClient = _httpClientFactory.CreateClient();
            var SignUpData = new { User.UserName, User.Email, User.Password, User.PhoneNumber, User.Address, User.Gender, User.Dob };
            TempData["SignUpData"] = JsonConvert.SerializeObject(SignUpData);
            var response = await httpClient.GetAsync($"https://localhost:5000/api/User/CheckSignUpEmail/{User.Email}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(User.From));
                email.To.Add(MailboxAddress.Parse(User.Email));
                email.Subject = "Vui lòng xác thực email của bạn";
                int number = GenerateRandom5DigitNumber();
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
            .code {{
                background-color: #da0f32;
                color: white;
                font-size: 24px;
                padding: 10px;
                text-align: center;
                border-radius: 5px;
                margin-bottom: 20px;
            }}
            p {{
                margin-bottom: 10px;
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
                <img src='https://inkythuatso.com/uploads/thumbnails/800/2021/09/cgv-logo-inkythuatso-1-14-16-41-01.jpg' alt='Email Header Image' style='max-width: 100%; height: auto;'>
            </div>
            <h2>Xác thực email của bạn</h2>
            <p>Cảm ơn bạn đã đăng ký. Dưới đây là mã xác thực duy nhất của bạn:</p>
            <div class='code'>{number}</div>
            <p>Vui lòng sử dụng mã này để hoàn tất quá trình đăng ký.</p>
            <p>Mã này sẽ hết hạn trong vòng 5 phút.</p>
            <p>Xin cảm ơn!</p>
        </div>
        <p class='footer'>CGV Cinema - Địa chỉ: 123 FPT University - Điện thoại: 0123 456 789</p>
    </body>
    </html>";


                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = emailBody };
                var db1 = _redisConnection.GetDatabase();
                await db1.StringSetAsync("VerifyEmail", number, TimeSpan.FromMinutes(5));
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(User.From, User.PasswordSendMail);
                smtp.Send(email);
                smtp.Disconnect(true);
                return RedirectToPage("/Register/VerifyRegister");
            }
            else if (response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Email already exists");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Register Failed!!!");
            }
            return Page();
        }
        private int GenerateRandom5DigitNumber()
        {
            Random rand = new Random();
            int min = 10000;
            int max = 99999;
            return rand.Next(min, max);
        }
    }
}
