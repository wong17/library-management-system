using System.Text.Json.Serialization;

namespace LibraryManagementSystem.WinUI.Entities.Dtos.Security;

public class UserRoleDto
{
    [JsonPropertyName("UserId")]
    public int UserId { get; set; }

    [JsonPropertyName("RoleId")]
    public int RoleId { get; set; }
}
