using Authenticate.API.Models;

namespace Authenticate.API.Sercurity
{
    public interface IJwtTokenService
    {
        (string token, DateTime expiresAtUtc) GenerateAccessToken(User user);
    }
}
