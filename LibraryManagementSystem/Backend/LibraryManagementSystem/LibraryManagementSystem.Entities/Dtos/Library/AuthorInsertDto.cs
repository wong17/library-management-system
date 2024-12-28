using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class AuthorInsertDto
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Nombre del Autor debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
        [RegularExpression(@"^[\p{L}\.\, ]+$", ErrorMessage = "Nombre del Autor solo puede tener letras, puntos, comas y espacios")]
        public string? Name { get; set; }

        [Required]
        public bool IsFormerGraduated { get; set; }
    }
}