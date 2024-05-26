using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.Library
{
    public class Book
    {
        [JsonPropertyName("BookId")]
        public int BookId { get; set; }

        [JsonPropertyName("ISBN10")]
        public string? ISBN10 { get; set; }

        [JsonPropertyName("ISBN13")]
        public string? ISBN13 { get; set; }

        [JsonPropertyName("Classification")]
        public string? Classification { get; set; }

        [JsonPropertyName("Title")]
        public string? Title { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonPropertyName("PublicationYear")]
        public short PublicationYear { get; set; }

        [JsonPropertyName("Image")]
        public byte[]? Image { get; set; }

        [JsonPropertyName("IsActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("PublisherId")]
        public int PublisherId { get; set; }

        [JsonPropertyName("CategoryId")]
        public int CategoryId { get; set; }

        [JsonPropertyName("NumberOfCopies")]
        public short NumberOfCopies { get; set; }

        [JsonPropertyName("BorrowedCopies")]
        public short BorrowedCopies { get; set; }

        [JsonPropertyName("IsAvailable")]
        public bool IsAvailable { get; set; }
    }
}
