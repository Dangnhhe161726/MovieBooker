using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MovieBooker_backend.Models
{
    public partial class bookMovieContext : DbContext
    {
        public bookMovieContext()
        {
        }

        public bookMovieContext(DbContextOptions<bookMovieContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; } = null!;
        public virtual DbSet<MovieCategory> MovieCategories { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Revervation> Revervations { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<Schedule> Schedules { get; set; } = null!;
        public virtual DbSet<Seat> Seats { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Theater> Theaters { get; set; } = null!;
        public virtual DbSet<TimeSlot> TimeSlots { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>(entity =>
            {
                entity.Property(e => e.MovieId)
                    .HasMaxLength(10)
                    .HasColumnName("movieId")
                    .IsFixedLength();

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.MovieTitle)
                    .HasMaxLength(50)
                    .HasColumnName("movieTitle");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.ReleaseDate)
                    .HasColumnType("datetime")
                    .HasColumnName("releaseDate");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Movies)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Movies_MovieCategory");
            });

            modelBuilder.Entity<MovieCategory>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.ToTable("MovieCategory");

                entity.Property(e => e.CategoryId).HasColumnName("categoryId");

                entity.Property(e => e.CategoryName).HasColumnName("categoryName");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.PaymentId).HasColumnName("paymentId");

                entity.Property(e => e.ReservationId).HasColumnName("reservationId");

                entity.Property(e => e.TotalAmount).HasColumnName("totalAmount");

                entity.HasOne(d => d.Reservation)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.ReservationId)
                    .HasConstraintName("FK_Payments_Revervations1");
            });

            modelBuilder.Entity<Revervation>(entity =>
            {
                entity.HasKey(e => e.ReservationId);

                entity.Property(e => e.ReservationId).HasColumnName("reservationId");

                entity.Property(e => e.ReservationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("reservationDate");

                entity.Property(e => e.SeatId).HasColumnName("seatId");

                entity.Property(e => e.StatusId).HasColumnName("statusId");

                entity.Property(e => e.TimeSlotId).HasColumnName("timeSlotId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Seat)
                    .WithMany(p => p.Revervations)
                    .HasForeignKey(d => d.SeatId)
                    .HasConstraintName("FK_Revervations_Seats");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Revervations)
                    .HasForeignKey(d => d.StatusId)
                    .HasConstraintName("FK_Revervations_Status");

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.Revervations)
                    .HasForeignKey(d => d.TimeSlotId)
                    .HasConstraintName("FK_Revervations_TimeSlots");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Revervations)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Revervations_Users");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.RoleName)
                    .HasMaxLength(50)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.RoomNumber)
                    .HasMaxLength(50)
                    .HasColumnName("roomNumber");

                entity.Property(e => e.TheaterId).HasColumnName("theaterId");

                entity.HasOne(d => d.Theater)
                    .WithMany(p => p.Rooms)
                    .HasForeignKey(d => d.TheaterId)
                    .HasConstraintName("FK_Rooms_Theaters");
            });

            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.HasKey(e => e.SchedulesId);

                entity.Property(e => e.SchedulesId)
                    .ValueGeneratedNever()
                    .HasColumnName("schedulesId");

                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("endDate");

                entity.Property(e => e.MovieId)
                    .HasMaxLength(10)
                    .HasColumnName("movieId")
                    .IsFixedLength();

                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("startDate");

                entity.Property(e => e.TheaterId).HasColumnName("theaterId");

                entity.Property(e => e.TimeSlotId).HasColumnName("timeSlotId");

                entity.HasOne(d => d.Movie)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.MovieId)
                    .HasConstraintName("FK_Schedules_Movies");

                entity.HasOne(d => d.Theater)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.TheaterId)
                    .HasConstraintName("FK_Schedules_Theaters");

                entity.HasOne(d => d.TimeSlot)
                    .WithMany(p => p.Schedules)
                    .HasForeignKey(d => d.TimeSlotId)
                    .HasConstraintName("FK_Schedules_TimeSlots");
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.Property(e => e.SeatId).HasColumnName("seatId");

                entity.Property(e => e.RoomId).HasColumnName("roomId");

                entity.Property(e => e.SeatNumber)
                    .HasMaxLength(10)
                    .HasColumnName("seatNumber")
                    .IsFixedLength();

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Seats)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_Seats_Rooms");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.StatusId).HasColumnName("statusId");

                entity.Property(e => e.StatusName)
                    .HasMaxLength(50)
                    .HasColumnName("statusName");
            });

            modelBuilder.Entity<Theater>(entity =>
            {
                entity.Property(e => e.TheaterId).HasColumnName("theaterId");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.TheaterName).HasColumnName("theaterName");
            });

            modelBuilder.Entity<TimeSlot>(entity =>
            {
                entity.Property(e => e.TimeSlotId).HasColumnName("timeSlotId");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("endTime");

                entity.Property(e => e.StartTime)
                    .HasColumnType("datetime")
                    .HasColumnName("startTime");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.Dob)
                    .HasColumnType("datetime")
                    .HasColumnName("dob");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(50)
                    .HasColumnName("phoneNumber");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
