using LibraryManagementSystem.Entities.Dtos.University;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class MonographDto
    {
        private int monographId;
        private string? classification;
        private string? title;
        private string? description;
        private string? tutor;
        private DateTime presentationDate;
        private string? image;
        private bool isActive;
        private bool isAvailable;
        private CareerDto? career;
        private List<AuthorDto>? authors = [];

        public int MonographId { get => monographId; set => monographId = value; }
        public string? Classification { get => classification; set => classification = value; }
        public string? Title { get => title; set => title = value; }
        public string? Description { get => description; set => description = value; }
        public string? Tutor { get => tutor; set => tutor = value; }
        public DateTime PresentationDate { get => presentationDate; set => presentationDate = value; }
        public string? Image { get => image; set => image = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public bool IsAvailable { get => isAvailable; set => isAvailable = value; }
        // Career
        public CareerDto? Career { get => career; set => career = value; }
        // Authors
        public List<AuthorDto>? Authors { get => authors; set => authors = value; }
    }
}
