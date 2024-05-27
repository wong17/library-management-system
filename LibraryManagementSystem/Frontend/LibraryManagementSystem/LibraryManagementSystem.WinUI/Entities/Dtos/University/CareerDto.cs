using System.Text.Json.Serialization;

namespace LibraryManagementSystem.WinUI.Entities.Dtos.University;

public class CareerDto
{
    [JsonPropertyName("CareerId")]
    public byte CareerId { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }
}
