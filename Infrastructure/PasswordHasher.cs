using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
   
        public class PasswordHasher
        {
            public string HashPassword(string password, out string salt)
            {
                salt = GenerateSalt();
                string hashedPassword = HashPasswordWithSalt(password, salt);
                return hashedPassword;
            }

            public bool VerifyPassword(string password, string hashedPassword, string salt)
            {
                string hashedPasswordToCompare = HashPasswordWithSalt(password, salt);
                return hashedPassword == hashedPasswordToCompare;
            }

            private string HashPasswordWithSalt(string password, string salt)
            {
                string saltedPassword = salt + password;
                return BCrypt.Net.BCrypt.HashPassword(saltedPassword, BCrypt.Net.BCrypt.GenerateSalt());
            }

            private string GenerateSalt()
            {
                return BCrypt.Net.BCrypt.GenerateSalt();
            }
        }

}
