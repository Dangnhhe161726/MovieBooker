using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MovieBooker.Pages.ForgotPassword
{
    public class ConfirmResetPasswordModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/Login");
        }
    }
}
