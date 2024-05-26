namespace LibraryManagementSystem.WinUI.Entities.Models.Library;

public class BookSubCategory
{
    public int BookId { get; set; }
    public int SubCategoryId { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime ModifiedOn { get; set; } = DateTime.Now;
}