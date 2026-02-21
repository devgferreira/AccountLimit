using System.Text.Json.Serialization;

namespace Authenticate.API.Models
{
    public class RegisterRequest
    {
        public string Username { get; set; } 
        public string Password { get; set; }
        [JsonIgnore]
        public string? Role { get; set; } 
    }
}
