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
            var listReservation = _context.Revervations.Include(r => r.Seat).ThenInclude(r => r.SeatType)
                                    .Select(r => new ReservationDTO()
                                    {
                                        ReservationId = r.ReservationId,
                                        UserId = r.UserId,
                                        SeatId = r.SeatId,
                                        TimeSlotId = r.TimeSlotId,
                                        MovieId = r.MovieId,
                                        SeatNumber = r.Seat.SeatNumber,
                                        RoomNumber = r.Seat.Room.RoomNumber,
                                        MovieTitle = r.Movie.MovieTitle,
                                        StartTime = r.TimeSlot.StartTime,
                                        EndTime = r.TimeSlot.EndTime,
                                        ReservationDate = r.ReservationDate,
                                        Price = r.TotalAmount,
                                        Status = r.Status,
                                        SeatType = r.Seat.SeatType.TypeName
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
                                        Price = r.TotalAmount,
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
            //Initialize table columns
            var dataTable = new DataTable();
            dataTable.Columns.Add("ReservationID", typeof(string));
            dataTable.Columns.Add("Movie Title", typeof(string));
            dataTable.Columns.Add("Price", typeof(string));
            dataTable.Columns.Add("Reservation Date", typeof(string));

            //Handle data for different types of report
            IEnumerable<ReservationDTO> resList = new List<ReservationDTO>();
            DateTime currentTime = DateTime.Now;
            DateTime startTime;
            switch (type)
            {
                case "weekly":
                    startTime = currentTime.AddDays(DayOfWeek.Monday - currentTime.DayOfWeek);
                    resList = GetReservationByTimePeriod(startTime, currentTime);
                    break;
                case "monthly":
                    startTime = new DateTime(currentTime.Year, currentTime.Month, 1);
                    resList = GetReservationByTimePeriod(startTime, currentTime);
                    break;
                case "yearly":
                    startTime = new DateTime(currentTime.Year, 1, 1);
                    resList = GetReservationByTimePeriod(startTime, currentTime);
                    break;
                default:
                    resList = GetAllReservation();
                    break;
            }

            //Push data into a dataTable
            foreach (ReservationDTO r in resList)
            {
                var row = dataTable.NewRow();
                row["ReservationID"] = r.ReservationId.ToString();
                row["Movie Title"] = r.MovieTitle;
                row["Price"] = r.Price.ToString();
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

            var totalRow = dataTable.NewRow();
            totalRow["ReservationID"] = "Total orders:";
            totalRow["Movie Title"] = resList.Count().ToString();
            totalRow["Price"] = "Total prices:";
            totalRow["Reservation Date"] = resList.Sum(r => r.Price).ToString();
            dataTable.Rows.Add(totalRow);

            return dataTable;
        }
        public void CreateReservation(CreateReservationDTO reservation)
        {
            var res = new Revervation
            {
                UserId = reservation.UserId,
                MovieId = reservation.MovieId,
                TimeSlotId = reservation.TimeSlotId,
                SeatId = reservation.SeatId,
                Status = reservation.Status,
                ReservationDate = reservation.ReservationDate,
                TotalAmount = reservation.TotalAmount,
            };
            _context.Revervations.Add(res);
            _context.SaveChanges();
        }
        public IEnumerable<ReservationDTO> GetReservationByTimePeriod(DateTime startTime, DateTime endTime)
        {
            var listReservation = _context.Revervations.Include(r => r.Seat).ThenInclude(r => r.SeatType)
                                    .Where(r => r.ReservationDate >= startTime && r.ReservationDate <= endTime)
                                    .OrderBy(r => r.ReservationDate)
                                    .Select(r => new ReservationDTO()
                                    {
                                        ReservationId = r.ReservationId,
                                        UserId = r.UserId,
                                        SeatId = r.SeatId,
                                        TimeSlotId = r.TimeSlotId,
                                        MovieId = r.MovieId,
                                        SeatNumber = r.Seat.SeatNumber,
                                        RoomNumber = r.Seat.Room.RoomNumber,
                                        MovieTitle = r.Movie.MovieTitle,
                                        StartTime = r.TimeSlot.StartTime,
                                        EndTime = r.TimeSlot.EndTime,
                                        ReservationDate = r.ReservationDate,
                                        Price = r.TotalAmount,
                                        Status = r.Status,
                                        SeatType = r.Seat.SeatType.TypeName
                                    })
                                    .ToList();
            return listReservation;
        }
    }
}
