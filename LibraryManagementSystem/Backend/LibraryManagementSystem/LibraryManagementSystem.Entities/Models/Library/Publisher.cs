using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.Library
{
    public class Publisher
    {
        [JsonPropertyName("PublisherId")]
        public int PublisherId { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }
    }
}
