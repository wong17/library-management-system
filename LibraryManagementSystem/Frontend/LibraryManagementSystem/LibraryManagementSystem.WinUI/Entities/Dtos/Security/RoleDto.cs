using System.Text.Json.Serialization;

namespace LibraryManagementSystem.WinUI.Entities.Dtos.Security;

public class RoleDto
{
    [JsonPropertyName("RoleId")]
    public int RoleId { get; set; }

    [JsonPropertyName("Name")]
    public string? Name { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }
}
