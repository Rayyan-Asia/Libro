﻿using Domain;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(int userId);
        Task<User> UpdateUserAsync(User user);
        Task<bool> UserExistsAsync(User user);
        Task<User?> GetUserProfileByIdAsync(int userId);
    }
}