using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class SubCategoryDto
    {
        [JsonPropertyName("SubCategoryId")]
        public int SubCategoryId { get; set; }

        [JsonPropertyName("Category")]
        public CategoryDto? Category { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }
    }
}
