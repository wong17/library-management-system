using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos
{
    public class PublisherInsertDto
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Nombre de la editorial debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
        public string? Name { get; set; }
    }
}
