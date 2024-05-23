using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class PublisherUpdateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id de la editorial debe ser mayor que 0")]
        public int PublisherId { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Nombre de la Editorial debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z.\- ]+$", ErrorMessage = "Nombre de la Editorial solo puede tener mayúsculas, minúsculas, guiones, puntos y espacios")]
        public string? Name { get; set; }
    }
}
