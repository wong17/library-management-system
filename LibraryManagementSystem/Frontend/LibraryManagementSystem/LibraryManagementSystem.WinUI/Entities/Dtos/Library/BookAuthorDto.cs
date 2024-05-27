using System.Text.Json.Serialization;

namespace LibraryManagementSystem.WinUI.Entities.Dtos.Library;

public class BookAuthorDto
{
    [JsonPropertyName("BookId")]
    public int BookId { get; set; }

    [JsonPropertyName("AuthorId")]
    public int AuthorId { get; set; }

    [JsonPropertyName("CreatedOn")]
    public DateTime CreatedOn { get; set; }

    [JsonPropertyName("ModifiedOn")]
    public DateTime ModifiedOn { get; set; }
 }

