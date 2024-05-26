using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.Security
{
    public class UserRole
    {
        [JsonPropertyName("UserId")]
        public int UserId { get; set; }

        [JsonPropertyName("RoleId")]
        public int RoleId { get; set; }
    }
}
