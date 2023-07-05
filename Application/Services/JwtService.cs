using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public static class JwtService
    {
        public static User GetUserFromPayload(string jwtToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);

            int userId = 0;
            var userIdClaim = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId))
            {
                userId = parsedUserId;
            }

            var roleString = jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(roleString))
            {
                return null;
            }

            if (Enum.TryParse<Role>(roleString, out var role) && userId != 0)
            {
                return new User { Id = userId, Role = role };
            }

            return null;
        }

        public static string GenerateJwt(User user, IConfiguration configuration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:SecretForKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
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
