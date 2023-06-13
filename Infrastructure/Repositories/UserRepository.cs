using Domain;
using Infrastructure.Interfaces;
using Libro.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibroDbContext _context;

        public UserRepository(LibroDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            int id = user.Id;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            if (GetUserByIdAsync(id) == null)
                return true;
            return false;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> UserExistsAsync(User user)
        {
            return await _context.Users.AnyAsync(r => r.Id == user.Id);
        }

        public async Task<User?> GetUserProfileByIdAsync(int userId)
        {
            var user = await _context.Users
              .Include(b => b.Loans)
              .Include(b => b.Reservations)
              .AsNoTracking()
              .Select(b => new User
              {
                  Id = b.Id,
                  Name = b.Name,
                  PhoneNumber = b.PhoneNumber,
                  Email = b.Email,


                  Loans = b.Loans
                    .Where(g => g.ReturnDate == null)
                    .Select(g => new Loan
                    {
                        Id = g.Id,
                        BookId = g.Id,
                        LoanDate = g.LoanDate,
                        DueDate = g.DueDate,
                    }).ToList(),

                  Reservations = b.Reservations.Where(b => b.IsPendingApproval == true)
                  .Select(g => new Reservation
                  {
                      Id = g.Id,
                      UserId = g.UserId,
                      BookId = g.Id,
                      ReservationDate = g.ReservationDate,
                  }).ToList(),

              }).SingleOrDefaultAsync(u=>u.Id == userId);

            return user;
        }
    }
}


