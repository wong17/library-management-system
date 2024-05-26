using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.University
{
    public class Career
    {
        [JsonPropertyName("CareerId")]
        public byte CareerId { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }
    }
}
