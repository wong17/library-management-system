using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.Security
{
    public class Role
    {
        [JsonPropertyName("RoleId")]
        public int RoleId { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }
    }
}
