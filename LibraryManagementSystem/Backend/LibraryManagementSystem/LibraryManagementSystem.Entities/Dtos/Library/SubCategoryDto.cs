namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class SubCategoryDto
    {
        public int SubCategoryId { get; set; }
        public CategoryDto? Category { get; set; }
        public string? Name { get; set; }
    }
}