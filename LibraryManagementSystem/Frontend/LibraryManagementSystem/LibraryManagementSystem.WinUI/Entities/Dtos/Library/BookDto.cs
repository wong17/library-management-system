using System.Text.Json.Serialization;

namespace LibraryManagementSystem.WinUI.Entities.Dtos.Library;

public class BookDto
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

    [JsonPropertyName("NumberOfCopies")]
    public short NumberOfCopies { get; set; }

    [JsonPropertyName("BorrowedCopies")]
    public short BorrowedCopies { get; set; }

    [JsonPropertyName("IsAvailable")]
    public bool IsAvailable { get; set; }

    // Publisher
    [JsonPropertyName("Publisher")]
    public PublisherDto? Publisher { get; set; }

    // Category
    [JsonPropertyName("Category")]
    public CategoryDto? Category { get; set; }

    // Authors
    [JsonPropertyName("Authors")]
    public List<AuthorDto>? Authors { get; set; } = new();

    // SubCategories
    [JsonPropertyName("SubCategories")]
    public List<SubCategoryDto>? SubCategories { get; set; } = new();
}
