namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class MonographAuthorDto
    {
        public int MonographId { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}