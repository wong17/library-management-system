using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos
{
    public class MonographAuthorInsertDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id de la monografía debe ser mayor que 0")]
        public int MonographId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del autor debe ser mayor que 0")]
        public int AuthorId { get; set; }
    }
}
