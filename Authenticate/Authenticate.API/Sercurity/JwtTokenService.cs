using Authenticate.API.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticate.API.Sercurity
{
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expireMinutes;

        public JwtTokenService()
        {
            _key = Environment.GetEnvironmentVariable("JWT_KEY") ?? "";
            _issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "";
            _audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "";

            _ = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIREMINUTES"), out _expireMinutes);
            _expireMinutes = _expireMinutes <= 0 ? 60 : _expireMinutes;

            if (string.IsNullOrWhiteSpace(_key)) throw new InvalidOperationException("JWT_KEY not configured.");
            if (string.IsNullOrWhiteSpace(_issuer)) throw new InvalidOperationException("JWT_ISSUER not configured.");
            if (string.IsNullOrWhiteSpace(_audience)) throw new InvalidOperationException("JWT_AUDIENCE not configured.");
        }

        public (string token, DateTime expiresAtUtc) GenerateAccessToken(User user)
        {
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_expireMinutes);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, expires);
        }
    }
}
