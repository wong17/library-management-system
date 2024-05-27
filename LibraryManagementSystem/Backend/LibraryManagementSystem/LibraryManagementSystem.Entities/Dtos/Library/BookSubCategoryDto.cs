using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class BookSubCategoryDto
    {
        [JsonPropertyName("BookId")]
        public int BookId { get; set; }

        [JsonPropertyName("SubCategoryId")]
        public int SubCategoryId { get; set; }

        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("ModifiedOn")]
        public DateTime ModifiedOn { get; set; }
    }
}
