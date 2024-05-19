using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos
{
    public class CategoryUpdateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id de la categoría debe ser mayor que 0")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Nombre de la categoría debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
        public string? Name { get; set; }
    }
}
