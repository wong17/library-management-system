using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Dtos.Security
{
    public class UserDto
    {
        [JsonPropertyName("UserId")]
        public int UserId { get; set; }

        [JsonPropertyName("UserName")]
        public string? UserName { get; set; }

        [JsonPropertyName("Email")]
        public string? Email { get; set; }

        [JsonPropertyName("Password")]
        public string? Password { get; set; }

        [JsonPropertyName("AccessToken")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("RefreshToken")]
        public string? RefreshToken { get; set; }

        [JsonPropertyName("RefreshTokenExpiryTime")]
        public DateTime RefreshTokenExpiryTime { get; set; }

        [JsonPropertyName("LockoutEnabled")]
        public bool LockoutEnabled { get; set; }

        [JsonPropertyName("AccessFailedCount")]
        public int AccessFailedCount { get; set; }
    }
}
