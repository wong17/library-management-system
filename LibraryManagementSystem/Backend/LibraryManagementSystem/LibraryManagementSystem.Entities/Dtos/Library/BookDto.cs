namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class BookDto
    {
        public int BookId { get; set; }
        public string? ISBN10 { get; set; }
        public string? ISBN13 { get; set; }
        public string? Classification { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public short PublicationYear { get; set; }
        public string? Image { get; set; }
        public bool IsActive { get; set; }
        public short NumberOfCopies { get; set; }
        public short BorrowedCopies { get; set; }
        public bool IsAvailable { get; set; }
        public PublisherDto? Publisher { get; set; }
        public CategoryDto? Category { get; set; }
        public List<AuthorDto>? Authors { get; set; } = new();
        public List<SubCategoryDto>? SubCategories { get; set; } = new();
    }
}