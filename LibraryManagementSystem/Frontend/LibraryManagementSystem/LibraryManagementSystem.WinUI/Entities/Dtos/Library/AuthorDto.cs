using System.Text.Json.Serialization;

namespace LibraryManagementSystem.WinUI.Entities.Dtos.Library;

public class AuthorDto
{
    [JsonPropertyName("AuthorId")]
    public int AuthorId { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("IsFormerGraduated")]
    public bool IsFormerGraduated { get; set; }
}
