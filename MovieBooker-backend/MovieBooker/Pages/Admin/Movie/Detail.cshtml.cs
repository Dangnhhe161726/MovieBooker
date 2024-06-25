using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MovieBooker.Pages.Admin.Movie
{
    public class DetailModel : PageModel
    {
        public void OnGet(int id)
        {
             id = 0;
        }
    }
}
