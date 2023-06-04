using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Infrastructure;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }

        public async Task<User> CreateUser(User user)
        {
            return await _userRepository.AddUser(user);
        }

        public async Task<User> UpdateUser(User user)
        {
            if (!await _userRepository.UserExists(user))
                return null;
            return await _userRepository.UpdateUser(user);
        }

        public async Task<bool> DeleteUser(int id)
        {
            // Ensure that the user exists before deleting
            var existingUser = await _userRepository.GetUserById(id);
            if (existingUser == null)
                return false;

            return await _userRepository.DeleteUser(existingUser);
        }

        public async Task<User?> Login(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
                return null;
            if (!PasswordHasher.VerifyPassword(password, user.Salt, user.HashedPassword))
            {
                return null;
            }
            return user;
        }

        public async Task<User?> RegisterUser(User user, string password)
        {
            var existingUser = await _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null) 
                return null;
            user.Salt = PasswordHasher.GenerateSalt();
            user.HashedPassword = PasswordHasher.ComputeHash(password, user.Salt);
            user = await _userRepository.AddUser(user);
            return user;
        }

        public async Task<bool> UserExists(string email)
        {
            var existingUser = await _userRepository.GetUserByEmail(email);
            if (existingUser != null)
                return true;
            return false;
        }

    }
}
