using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class CategoryDto
    {
        [JsonPropertyName("CategoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }
    }
}
