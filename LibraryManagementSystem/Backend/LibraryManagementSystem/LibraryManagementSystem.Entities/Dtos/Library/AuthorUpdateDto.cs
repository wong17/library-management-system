using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class AuthorUpdateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del autor debe ser mayor que 0")]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Nombre del Autor debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\.\, ]+$", ErrorMessage = "Nombre del Autor solo puede tener letras, puntos, comas y espacios")]
        public string? Name { get; set; }

        [Required]
        public bool IsFormerGraduated { get; set; }
    }
}
