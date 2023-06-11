using Domain;
using Infrastructure.Interfaces;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
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
                .Where(b=>b.IsPendingApproval == true)
                .ToListAsync();

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
    }
}
