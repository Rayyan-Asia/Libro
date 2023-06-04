using Domain;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUser(User user);
        Task<bool> DeleteUser(User user);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(int userId);
        Task<User> UpdateUser(User user);
        Task<bool> UserExists(User user);
    }
}