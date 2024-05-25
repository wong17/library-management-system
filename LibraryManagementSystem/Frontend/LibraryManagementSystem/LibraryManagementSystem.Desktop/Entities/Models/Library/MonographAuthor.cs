namespace LibraryManagementSystem.Desktop.Entities.Models.Library
{
    public class MonographAuthor
    {
        public int MonographId { get; set; }
        public int AuthorId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime ModifiedOn { get; set; } = DateTime.Now;
    }
}
