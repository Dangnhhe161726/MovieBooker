using MovieBooker_backend.DTO;
using Microsoft.AspNetCore.Mvc;
using MovieBooker_backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Data;

namespace MovieBooker_backend.Repositories.ReservationRepository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly bookMovieContext _context;

        public ReservationRepository(bookMovieContext context)
        {
            _context = context;
        }

        public IEnumerable<ReservationDTO> GetAllReservation()
        {
            var listReservation = _context.Revervations
                                    .Select(r => new ReservationDTO()
                                    {
                                        ReservationId = r.ReservationId,
                                        UserId = r.UserId,
                                        SeatNumber = r.Seat.SeatNumber,
                                        RoomNumber = r.Seat.Room.RoomNumber,
                                        MovieTitle = r.Movie.MovieTitle,
                                        StartTime = r.TimeSlot.StartTime,
                                        EndTime = r.TimeSlot.EndTime,
                                        ReservationDate = r.ReservationDate,
                                        Price = r.Movie.Price,
                                        Status = r.Status
                                    })
                                    .ToList();
            return listReservation;
        }
        public ReservationDTO GetReservationById(int resId)
        {
            var res = _context.Revervations
                                    .Where(r => r.ReservationId == resId)
                                    .Select(r => new ReservationDTO()
                                    {
                                        ReservationId = r.ReservationId,
                                        UserId = r.UserId,
                                        SeatNumber = r.Seat.SeatNumber,
                                        RoomNumber = r.Seat.Room.RoomNumber,
                                        MovieTitle = r.Movie.MovieTitle,
                                        StartTime = r.TimeSlot.StartTime,
                                        EndTime = r.TimeSlot.EndTime,
                                        ReservationDate = r.ReservationDate,
                                        Price = r.Movie.Price,
                                        Status = r.Status
                                    })
                                    .FirstOrDefault();
            return res;
        }
        public async Task<int> AddNewReservation(Revervation res)
        {
            var reservation = new Revervation
            {
                ReservationId = res.ReservationId,
                UserId = res.UserId,
                MovieId = res.MovieId,
                TimeSlotId = res.TimeSlotId,
                SeatId = res.SeatId,
                ReservationDate = res.ReservationDate,
                Status = res.Status
            };

            _context.Revervations.Add(reservation);
            var result = await _context.SaveChangesAsync();
            return result;
        }
        public DataTable GenerateReport(string type)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("ReservationID", typeof(int));
            dataTable.Columns.Add("Movie Title", typeof(string));
            dataTable.Columns.Add("Price", typeof(double));
            dataTable.Columns.Add("Reservation Date", typeof(string));

            
            IEnumerable<ReservationDTO> resList = GetAllReservation();
            foreach (ReservationDTO r in resList)
            {
                var row = dataTable.NewRow();
                row["ReservationID"] = r.ReservationId;
                row["Movie Title"] = r.MovieTitle;
                row["Price"] = r.Price;
                // Ensure the ReservationDate is of type DateTime
                if (r.ReservationDate != null)
                {
                    row["Reservation Date"] = r.ReservationDate.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    row["Reservation Date"] = DBNull.Value;
                }
                
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }
    }
}
