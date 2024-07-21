using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieBooker.DTO;
using MovieBooker.Models;

namespace MovieBooker.Pages.Users.Cart
{
    public class BookSeatModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public TicketDTO ticket {  get; set; }
        public List<ScheduleDTO> schedules { get; set; }
        public List<SeatDTO> seats { get; set; }    
        public List<ReservationDTO> reservations { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            string? date = ticket.start;
            int? movieId = ticket.movieId;
            int? theartr = ticket.theartr;
            int? slot = ticket.timeslot;
            HttpClient _httpClient = new HttpClient();
            HttpResponseMessage response2 = await
            _httpClient.GetAsync($"https://localhost:5000/api/Schedule/GetSchedule?$filter=scheduleDate eq {date} and movieId eq {movieId} and timeSlotId eq {slot} and theaterId eq {theartr}");
            if (response2.IsSuccessStatusCode)
            {
                schedules = await response2.Content.ReadFromJsonAsync<List<ScheduleDTO>>();
            }
            int roomId = (int)schedules.First().RoomId;
            HttpResponseMessage response3 = await _httpClient.GetAsync($"https://localhost:5000/api/Seat/GetSeat?$filter=theatersId eq {theartr} and roomId eq {roomId}");
            if (response3.IsSuccessStatusCode)
            {
                seats = await response3.Content.ReadFromJsonAsync<List<SeatDTO>>();
            }
            HttpResponseMessage response4 = await _httpClient.GetAsync($"https://localhost:5000/api/Reservation?$filter=reservationDate eq {date} and movieId eq {movieId} and timeSlotId eq {slot} ");
            if (response4.IsSuccessStatusCode)
            {
                reservations = await response4.Content.ReadFromJsonAsync<List<ReservationDTO>>();
            }
            var bookedSeats = new HashSet<int>(reservations.Select(r => r.SeatId.GetValueOrDefault()));
            foreach (var seat in seats)
            {
                seat.IsBooked = bookedSeats.Contains(seat.SeatId);
            }

            return Page();  
        }
    }
}
