using Domain;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<User> CreateUser(User user);
        Task<bool> DeleteUser(int id);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(int id);
        Task<User?> Login(string email, string password);
        Task<User?> RegisterUser(User user, string password);
        Task<User> UpdateUser(User user);
        Task<bool> UserExists(string email);
    }
}