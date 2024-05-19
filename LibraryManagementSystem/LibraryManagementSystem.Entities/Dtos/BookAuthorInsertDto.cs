using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos
{
    public class BookAuthorInsertDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del libro debe ser mayor que 0")]
        public int BookId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del autor debe ser mayor que 0")]
        public int AuthorId { get; set; }
    }
}
