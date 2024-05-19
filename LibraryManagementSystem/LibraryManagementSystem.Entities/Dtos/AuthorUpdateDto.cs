using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos
{
    public class AuthorUpdateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del autor debe ser mayor que 0")]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Nombre del autor debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
        public string? Name { get; set; }

        [Required]
        public bool IsFormerGraduated { get; set; }
    }
}
