namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class BookAuthorDto
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}