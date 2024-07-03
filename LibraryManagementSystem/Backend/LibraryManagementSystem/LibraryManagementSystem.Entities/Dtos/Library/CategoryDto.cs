namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class CategoryDto
    {
        private int categoryId;
        private string? name;

        public int CategoryId { get => categoryId; set => categoryId = value; }
        public string? Name { get => name; set => name = value; }
    }
}
