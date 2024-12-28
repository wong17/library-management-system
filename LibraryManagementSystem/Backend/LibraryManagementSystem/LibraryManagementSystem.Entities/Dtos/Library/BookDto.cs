namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class BookDto
    {
        private int bookId;
        private string? iSBN10;
        private string? iSBN13;
        private string? classification;
        private string? title;
        private string? description;
        private short publicationYear;
        private string? image;
        private bool isActive;
        private short numberOfCopies;
        private short borrowedCopies;
        private bool isAvailable;
        private PublisherDto? publisher;
        private CategoryDto? category;
        private List<AuthorDto>? authors = [];
        private List<SubCategoryDto>? subCategories = [];

        public int BookId { get => bookId; set => bookId = value; }
        public string? ISBN10 { get => iSBN10; set => iSBN10 = value; }
        public string? ISBN13 { get => iSBN13; set => iSBN13 = value; }
        public string? Classification { get => classification; set => classification = value; }
        public string? Title { get => title; set => title = value; }
        public string? Description { get => description; set => description = value; }
        public short PublicationYear { get => publicationYear; set => publicationYear = value; }
        public string? Image { get => image; set => image = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public short NumberOfCopies { get => numberOfCopies; set => numberOfCopies = value; }
        public short BorrowedCopies { get => borrowedCopies; set => borrowedCopies = value; }
        public bool IsAvailable { get => isAvailable; set => isAvailable = value; }

        // Publisher
        public PublisherDto? Publisher { get => publisher; set => publisher = value; }

        // Category
        public CategoryDto? Category { get => category; set => category = value; }

        // Authors
        public List<AuthorDto>? Authors { get => authors; set => authors = value; }

        // SubCategories
        public List<SubCategoryDto>? SubCategories { get => subCategories; set => subCategories = value; }
    }
}