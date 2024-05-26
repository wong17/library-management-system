using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.Library
{
    public class Author
    {
        [JsonPropertyName("AuthorId")]
        public int AuthorId { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }
        
        [JsonPropertyName("IsFormerGraduated")]
        public bool IsFormerGraduated { get; set; }
    }
}
