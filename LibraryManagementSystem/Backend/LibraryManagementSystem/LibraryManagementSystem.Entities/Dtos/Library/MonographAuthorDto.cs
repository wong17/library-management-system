using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class MonographAuthorDto
    {
        [JsonPropertyName("MonographId")]
        public int MonographId { get; set; }

        [JsonPropertyName("AuthorId")]
        public int AuthorId { get; set; }

        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("ModifiedOn")]
        public DateTime ModifiedOn { get; set; }
    }
}
