namespace LibraryManagementSystem.Entities.Models.Library
{
    public class Book
    {
        private int bookId;
        private string? iSBN10;
        private string? iSBN13;
        private string? classification;
        private string? title;
        private string? description;
        private short publicationYear;
        private byte[]? image;
        private bool isActive;
        private int publisherId;
        private int categoryId;
        private short numberOfCopies;
        private short borrowedCopies;
        private bool isAvailable;

        public int BookId { get => bookId; set => bookId = value; }
        public string? ISBN10 { get => iSBN10; set => iSBN10 = value; }
        public string? ISBN13 { get => iSBN13; set => iSBN13 = value; }
        public string? Classification { get => classification; set => classification = value; }
        public string? Title { get => title; set => title = value; }
        public string? Description { get => description; set => description = value; }
        public short PublicationYear { get => publicationYear; set => publicationYear = value; }
        public byte[]? Image { get => image; set => image = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public int PublisherId { get => publisherId; set => publisherId = value; }
        public int CategoryId { get => categoryId; set => categoryId = value; }
        public short NumberOfCopies { get => numberOfCopies; set => numberOfCopies = value; }
        public short BorrowedCopies { get => borrowedCopies; set => borrowedCopies = value; }
        public bool IsAvailable { get => isAvailable; set => isAvailable = value; }
    }
}
