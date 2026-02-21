using Authenticate.API.Models;
using System.Collections.Concurrent;

namespace Authenticate.API.Repository
{
    public sealed class InMemoryUserRepository : IInMemoryUserRepository
    {
        private readonly ConcurrentDictionary<string, User> _users =
            new(StringComparer.OrdinalIgnoreCase);

        public Task<User?> GetByUsernameAsync(string username)
        {
            username = (username ?? "").Trim();
            if (string.IsNullOrWhiteSpace(username))
                return Task.FromResult<User?>(null);

            _users.TryGetValue(username, out var user);
            return Task.FromResult(user);
        }

        public Task CreateAsync(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));
            user.Username = (user.Username ?? "").Trim();

            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username is required.");

            if (!_users.TryAdd(user.Username, user))
                throw new InvalidOperationException("User already exists.");

            return Task.CompletedTask;
        }
    }
}
