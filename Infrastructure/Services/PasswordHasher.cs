﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using AutoDependencyRegistration.Attributes;

namespace Infrastructure.Services
{
    [RegisterClassAsScoped]
    public class PasswordHasher : IPasswordHasher
    {
        public string ComputeHash(string password, string salt, int iteration = 4)
        {
            if (iteration <= 1) return password;

            using var sha256 = SHA256.Create();
            var passwordSalt = $"{password}{salt}";
            var byteValue = Encoding.UTF8.GetBytes(passwordSalt);
            var byteHash = sha256.ComputeHash(byteValue);
            var hash = Convert.ToBase64String(byteHash);

            return ComputeHash(hash, salt, iteration - 1);
        }

        public string GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var byteSalt = new byte[16];
            rng.GetBytes(byteSalt);
            var salt = Convert.ToBase64String(byteSalt);

            return salt;
        }

        public bool VerifyPassword(string password, string salt, string passwordToCompare, int iteration = 4)
        {
            string generatedPassword = ComputeHash(password, salt, iteration);
            return generatedPassword.Equals(passwordToCompare);
        }

    }

}
