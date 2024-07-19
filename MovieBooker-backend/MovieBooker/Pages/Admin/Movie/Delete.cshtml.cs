using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MovieBooker.Pages.Admin.Movie
{
	public class DeleteModel : PageModel
	{
		private readonly IHttpClientFactory _httpClientFactory;

		public DeleteModel(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
		}

		public async Task<IActionResult> OnPostAsync(int id, string publicId, int movieId)
		{
			try
			{
				if (string.IsNullOrEmpty(publicId))
				{
					throw new ArgumentNullException(nameof(publicId), "Public ID is required.");
				}

				string encodedPublicId = Uri.EscapeDataString(publicId);

				using (var client = _httpClientFactory.CreateClient())
				{
					// Delete image on Cloudinary
					var responseOnCloud = await client.DeleteAsync($"https://localhost:5000/api/Images/{encodedPublicId}");
					responseOnCloud.EnsureSuccessStatusCode();

					// Read response from Cloudinary API
					var cloudResponseContent = await responseOnCloud.Content.ReadAsStringAsync();
					if (!cloudResponseContent.ToLower().Contains("ok"))
					{
						throw new Exception("Failed to delete image on Cloudinary.");
					}

					// Delete image in Database
					var responseRemoveOnData = await client.DeleteAsync($"https://localhost:5000/api/MovieImage/{id}");
					responseRemoveOnData.EnsureSuccessStatusCode();
				}

				return RedirectToPage("/Admin/Movie/Edit", new { id = movieId });
			}
			catch (Exception ex)
			{
				// Log or handle the exception
				Console.WriteLine($"Exception: {ex.Message}");
				return RedirectToPage("/Admin/Movie/Edit", new { id = movieId });
			}
		}

		public async Task<IActionResult> OnPostDeleteHandler(int id)
		{
			using (var client = _httpClientFactory.CreateClient())
			{
				var response = await client.DeleteAsync($"https://localhost:5000/api/Movie/{id}");
				response.EnsureSuccessStatusCode();
			}
			return RedirectToPage("/Admin/Movie/Index");
		}
	}
}
