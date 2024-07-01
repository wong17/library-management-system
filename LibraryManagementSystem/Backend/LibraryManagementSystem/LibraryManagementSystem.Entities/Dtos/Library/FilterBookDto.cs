using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class FilterBookDto
    {
        // Authors
        [Required]
        [JsonPropertyName("authors")]
        public IEnumerable<AuthorDto>? Authors { get; set; }

        // Categories
        [Required]
        [JsonPropertyName("categories")]
        public IEnumerable<CategoryDto>? Categories { get; set; }

        // Publishers
        [Required]
        [JsonPropertyName("publishers")]
        public IEnumerable<PublisherDto>? Publishers { get; set; }

        // SubCategories
        [Required]
        [JsonPropertyName("subCategories")]
        public IEnumerable<SubCategoryFilterDto>? SubCategories { get; set; }

        // Publication year
        [Required]
        [JsonPropertyName("publicationYear")]
        public short? PublicationYear { get; set; } = null;
    }
}
