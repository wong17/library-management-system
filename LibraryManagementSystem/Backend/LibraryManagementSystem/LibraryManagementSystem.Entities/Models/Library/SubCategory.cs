using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.Library
{
    public class SubCategory
    {
        [JsonPropertyName("SubCategoryId")]
        public int SubCategoryId { get; set; }

        [JsonPropertyName("CategoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }
    }
}
