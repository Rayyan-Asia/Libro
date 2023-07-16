using Domain;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public interface IJwtService
    {
        string GenerateJwt(User user, IConfiguration configuration);
        User GetUserFromPayload(string jwtToken);
    }
}