using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class PublisherDto
    {
        [JsonPropertyName("PublisherId")]
        public int PublisherId { get; set; }

        [JsonPropertyName("Name")]
        public string? Name { get; set; }
    }
}
