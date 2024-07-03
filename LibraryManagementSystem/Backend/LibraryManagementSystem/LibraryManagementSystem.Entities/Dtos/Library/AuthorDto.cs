namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class AuthorDto
    {
        private int authorId;
        private string? name;
        private bool isFormerGraduated;

        public int AuthorId { get => authorId; set => authorId = value; }
        public string? Name { get => name; set => name = value; }
        public bool IsFormerGraduated { get => isFormerGraduated; set => isFormerGraduated = value; }
    }
}
