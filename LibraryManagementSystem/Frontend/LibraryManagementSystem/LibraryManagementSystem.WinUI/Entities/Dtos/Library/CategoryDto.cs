using System.Text.Json.Serialization;

namespace LibraryManagementSystem.WinUI.Entities.Dtos.Library;

public class CategoryDto
{
    [JsonPropertyName("CategoryId")]
    public int CategoryId { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }
}
