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

        public async Task<User?> GetUserById(int userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUser(User user)
        {
            int id = user.Id;
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            if (GetUserById(id) == null)
                return true;
            return false;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> UserExists(User user)
        {
            return await _context.Users.AnyAsync(r => r.Id == user.Id);
        }
    }
}


