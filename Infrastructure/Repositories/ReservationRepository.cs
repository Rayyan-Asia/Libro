using Application;
using Application.Interfaces;
using AutoDependencyRegistration.Attributes;
using Domain;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    [RegisterClassAsScoped]
    public class ReservationRepository : IReservationRepository
    {
        private readonly LibroDbContext _context;
        public ReservationRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation?> GetReservationByIdAsync(int reservationId)
        {
            return await _context.Reservations.FirstOrDefaultAsync(r => r.Id == reservationId);
        }

        public async Task<Reservation?> GetReservationByUserIdAndBookIdAsync(int userId, int bookId)
        {
            return await _context.Reservations.FirstOrDefaultAsync(r => r.UserId ==userId && r.BookId == bookId && r.IsPendingApproval==true);
        }

        public async Task<Reservation> AddReservationAsync(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
            return reservation;
        }

        public async Task<(PaginationMetadata, List<Reservation>)> GetAllReservationsAsync(int pageNumber, int pageSize)
        {
            var reservations = await _context.Reservations.AsNoTracking()
                .Where(r=>r.IsPendingApproval == true).OrderBy(r=>r.ReservationDate)
                .ToListAsync();

            var count = reservations.Count;

            var pageCount = (int)Math.Ceiling((double)count / pageSize) - 1;

            if (pageNumber > pageCount) pageNumber = pageCount;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber + 1,
                PageSize = pageSize
            };
            var take = pageSize - count % pageSize;
            var filteredDoctors = reservations.OrderBy(r => r.ReservationDate)
                .Skip(pageNumber * pageSize).Take(take).ToList();
            return (metadata, filteredDoctors);
        }

        public async Task<(PaginationMetadata, List<Reservation>)> GetAllReservationsByUserIdAsync(int pageNumber, int pageSize, int userId)
        {
            var reservations = await _context.Reservations.AsNoTracking().Where(b=>b.UserId == userId).ToListAsync();

            var count = reservations.Count;

            var metadata = new PaginationMetadata()
            {
                ItemCount = count,
                PageCount = pageNumber,
                PageSize = pageSize
            };

            var filteredDoctors = reservations.OrderBy(b => b.ReservationDate)
                .Skip(pageNumber * pageSize).Take(pageSize).ToList();
            return (metadata, filteredDoctors);
        }

        public async Task<Reservation?> ApproveReservationByIdAsync(int reservationId)
        {
            var reservation = await GetReservationByIdAsync(reservationId);
            if (reservation == null || reservation.IsPendingApproval == false)
                return null;
            reservation.IsPendingApproval = false;
            reservation.IsApproved = true;
            await _context.SaveChangesAsync();
            return reservation;
        }
        public async Task<bool> IsPatronEligableForReservationAsync(int userId)
        {
            var count = await _context.Reservations.Where(b=> b.IsPendingApproval == true && b.UserId == userId).CountAsync();
            return count < 5;
        }
        public async Task<Reservation?> RejectReservationByIdAsync(int reservationId)
        {
            var reservation = await GetReservationByIdAsync(reservationId);
            if (reservation == null || reservation.IsPendingApproval == false)
                return null;
            reservation.IsPendingApproval = false;
            reservation.IsApproved = false;
            await _context.SaveChangesAsync();
            return reservation;
        }

    }
}
