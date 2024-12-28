namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class SubCategoryDto
    {
        private int subCategoryId;
        private CategoryDto? category;
        private string? name;

        public int SubCategoryId { get => subCategoryId; set => subCategoryId = value; }
        public CategoryDto? Category { get => category; set => category = value; }
        public string? Name { get => name; set => name = value; }
    }
}