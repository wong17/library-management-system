namespace LibraryManagementSystem.Entities.Models.Library
{
    public class SubCategory
    {
        private int subCategoryId;
        private int categoryId;
        private string? name;

        public int SubCategoryId { get => subCategoryId; set => subCategoryId = value; }
        public int CategoryId { get => categoryId; set => categoryId = value; }
        public string? Name { get => name; set => name = value; }
    }
}
