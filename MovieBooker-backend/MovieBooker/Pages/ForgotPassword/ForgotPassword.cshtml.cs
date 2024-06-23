using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit;
using MovieBooker.Models;
using StackExchange.Redis;
using MailKit.Net.Smtp;

namespace MovieBooker.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ForgotPasswordModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [BindProperty]
        public string emails { get; set; } = default!;
        [BindProperty]
        public User User { get; set; } = default!;
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://localhost:5000/api/User/CheckSignUpEmail/{emails}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                ModelState.AddModelError(string.Empty, "Email Not Found. Try Again!!!");
                return Page();
            }
            else
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(User.From));
                email.To.Add(MailboxAddress.Parse(emails));
                email.Subject = "Reset Your Password";
                string number = GenerateRandom();
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
            <h2>Mật khẩu mới của bạn</h2>
            <p>Dưới đây là mật khẩu đăng nhập của bạn:</p>
            <div class='code'>{number}</div>
            <p>Vui lòng sử dụng mật khẩu này để đăng nhập vào hệ thống. Bạn có thể đổi mật khẩu trong trang thông tin cá nhân</p>
            <p>Xin cảm ơn!</p>
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

                var response2 = await client.PutAsync($"https://localhost:5000/api/User/ResetPassword/{emails}/{number}", null);
                if (response2.IsSuccessStatusCode ) {
                    return RedirectToPage("/ForgotPassword/ConfirmResetPassword");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Reset Password failed. Try Again!!!");
                    return Page();
                }
              
            }
        }

        private string GenerateRandom()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random rand = new Random();
            char[] stringChars = new char[8];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[rand.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
