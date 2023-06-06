namespace Application
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Domain;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;

    public static class TokenHandler
    {
        public static User? GetUserFromPayload(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);
            int userId = 0;
            try
            {
               userId = int.Parse(jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "userId").Value);
            }
            catch (Exception ex) {
               
            }
              
            var roleString = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "role").Value;
            if (roleString == null)
            {
                return null;
            }

            Role role = (Role)Enum.Parse(typeof(Role), roleString);
            if (userId != 0 && role != null)
                return new User {Id = userId, Role = role};
            return null;
        }

        public static string GenerateJwt(User user, IConfiguration configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("role", user.Role.ToString())
            };

            var jwtSecurityToken = new JwtSecurityToken(
                configuration["Authentication:Issuer"],
                configuration["Authentication:Audience"],
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return token;
        }

    }

}
