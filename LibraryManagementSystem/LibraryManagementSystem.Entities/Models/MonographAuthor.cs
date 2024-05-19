namespace LibraryManagementSystem.Entities.Models
{
    public class MonographAuthor
    {
        public int MonographId { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
