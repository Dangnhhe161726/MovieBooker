using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.Models;
using Newtonsoft.Json;

namespace MovieBooker.Pages
{
    public class ExternalLoginCallbackModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalLoginCallbackModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync("https://localhost:5000/api/User/ExternalLoginCallback");

            if (response.IsSuccessStatusCode)
            {
                var tokens = await response.Content.ReadAsStringAsync();
                var tokenModel = JsonConvert.DeserializeObject<TokenModel>(tokens);

                HttpContext.Response.Cookies.Append("Token", tokenModel.AccessToken);
                HttpContext.Response.Cookies.Append("RefreshToken", tokenModel.RefreshToken);

                return LocalRedirect(returnUrl ?? "/Index");
            }

            return BadRequest();
        }
    }
}
