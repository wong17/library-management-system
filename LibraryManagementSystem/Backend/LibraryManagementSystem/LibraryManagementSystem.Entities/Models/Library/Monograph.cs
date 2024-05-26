using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.Library
{
    public class Monograph
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

        [JsonPropertyName("CareerId")]
        public int CareerId { get; set; }

        [JsonPropertyName("IsActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("IsAvailable")]
        public bool IsAvailable { get; set; }
    }
}
