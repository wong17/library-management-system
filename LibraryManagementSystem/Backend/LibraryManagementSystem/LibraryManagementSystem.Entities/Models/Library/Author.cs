namespace LibraryManagementSystem.Entities.Models.Library
{
    public class Author
    {
        private int authorId;
        private string? name;
        private bool isFormerGraduated;

        public int AuthorId { get => authorId; set => authorId = value; }
        public string? Name { get => name; set => name = value; }
        public bool IsFormerGraduated { get => isFormerGraduated; set => isFormerGraduated = value; }
    }
}