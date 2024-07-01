using LibraryManagementSystem.Entities.Dtos.University;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class FilterMonographDto
    {
        // Authors
        [Required]
        [JsonPropertyName("authors")]
        public IEnumerable<AuthorDto>? Authors { get; set; }

        // Authors
        [Required]
        [JsonPropertyName("careers")]
        public IEnumerable<CareerDto>? Careers { get; set; }

        [Required]
        [JsonPropertyName("beginPresentationDate")]
        public DateTime? BeginPresentationDate { get; set; }

        [Required]
        [JsonPropertyName("endPresentationDate")]
        public DateTime? EndPresentationDate { get; set; }
    }
}
