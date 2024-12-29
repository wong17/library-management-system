using LibraryManagementSystem.Entities.Dtos.University;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class MonographDto
    {
        public int MonographId { get; set; }
        public string? Classification { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Tutor { get; set; }
        public DateTime PresentationDate { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public bool IsAvailable { get; set; }
        public CareerDto? Career { get; set; }
        public List<AuthorDto>? Authors { get; set; } = new();
    }
}