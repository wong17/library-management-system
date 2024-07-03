namespace LibraryManagementSystem.Entities.Models.Library
{
    public class Monograph
    {
        private int monographId;
        private string? classification;
        private string? title;
        private string? description;
        private string? tutor;
        private DateTime presentationDate;
        private byte[]? image;
        private int careerId;
        private bool isActive;
        private bool isAvailable;

        public int MonographId { get => monographId; set => monographId = value; }
        public string? Classification { get => classification; set => classification = value; }
        public string? Title { get => title; set => title = value; }
        public string? Description { get => description; set => description = value; }
        public string? Tutor { get => tutor; set => tutor = value; }
        public DateTime PresentationDate { get => presentationDate; set => presentationDate = value; }
        public byte[]? Image { get => image; set => image = value; }
        public int CareerId { get => careerId; set => careerId = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public bool IsAvailable { get => isAvailable; set => isAvailable = value; }
    }
}
