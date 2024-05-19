namespace LibraryManagementSystem.Entities.Models
{
    public class BookSubCategory
    {
        public int BookId { get; set; }
        public int SubCategoryId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}
