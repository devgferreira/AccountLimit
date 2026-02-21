using Authenticate.API.Models;
using Authenticate.API.Repository;
using Authenticate.API.Sercurity;

namespace Authenticate.API.Service
{
    public sealed class AuthService : IAuthService
    {
        private readonly IInMemoryUserRepository _user;
        private readonly IJwtTokenService _jwt;

        public AuthService(IInMemoryUserRepository users, IJwtTokenService jwt)
        {
            _user = users;
            _jwt = jwt;
        }

        public async Task<LoginResponse> RegisterAsync(RegisterRequest request)
        {
            var username = (request.Username ?? "").Trim();
            var password = request.Password ?? "";
            var role = "USER";

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.");

            var users = await _user.GetByUsernameAsync(username);
            if (users != null)
                throw new InvalidOperationException("User already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Role = role
            };

            await _user.CreateAsync(user);

            var (token, exp) = _jwt.GenerateAccessToken(user);

            return new LoginResponse
            {
                AccessToken = token,
                ExpiresAt = exp
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var username = (request.Username ?? "").Trim();
            var password = request.Password ?? "";

            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.");

            var user = await _user.GetByUsernameAsync(username);

            if (user is null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var validPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!validPassword)
                throw new UnauthorizedAccessException("Invalid credentials.");

            var (token, exp) = _jwt.GenerateAccessToken(user);

            return new LoginResponse
            {
                AccessToken = token,
                ExpiresAt = exp
            };
        }
    }
}
