namespace LibraryManagementSystem.Entities.Models.Library
{
    public class Book
    {
        public int BookId { get; set; }
        public string? ISBN10 { get; set; }
        public string? ISBN13 { get; set; }
        public string? Classification { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public short PublicationYear { get; set; }
        public byte[]? Image { get; set; }
        public bool IsActive { get; set; }
        public int PublisherId { get; set; }
        public int CategoryId { get; set; }
        public short NumberOfCopies { get; set; }
        public short BorrowedCopies { get; set; }
        public bool IsAvailable { get; set; }
    }
}