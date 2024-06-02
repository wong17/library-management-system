using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Dtos.Security
{
    public class UserLogInDto
    {
        [JsonPropertyName("UserName")]
        public string? UserName { get; set; }

        [JsonPropertyName("Password")]
        public string? Password { get; set; }
    }
}
