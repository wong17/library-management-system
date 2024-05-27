using LibraryManagementSystem.WinUI.Entities.Dtos.University;
using System.Text.Json.Serialization;

namespace LibraryManagementSystem.WinUI.Entities.Dtos.Library;

public class MonographDto
{
    [JsonPropertyName("MonographId")]
    public int MonographId { get; set; }

    [JsonPropertyName("Classification")]
    public string? Classification { get; set; }

    [JsonPropertyName("Title")]
    public string? Title { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("Tutor")]
    public string? Tutor { get; set; }

    [JsonPropertyName("PresentationDate")]
    public DateTime PresentationDate { get; set; }

    [JsonPropertyName("Image")]
    public byte[]? Image { get; set; }

    [JsonPropertyName("IsActive")]
    public bool IsActive { get; set; }

    [JsonPropertyName("IsAvailable")]
    public bool IsAvailable { get; set; }

    // Career
    [JsonPropertyName("Career")]
    public CareerDto? Career { get; set; }

    // Authors
    [JsonPropertyName("Authors")]
    public List<AuthorDto>? Authors { get; set; } = new();
}
