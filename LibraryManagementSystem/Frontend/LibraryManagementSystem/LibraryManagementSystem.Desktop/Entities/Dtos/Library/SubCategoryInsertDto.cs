using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class SubCategoryInsertDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id de la categoría debe ser mayor que 0")]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Nombre de la Sub categoría debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z\- ]+$", ErrorMessage = "Nombre de la Sub categoría solo puede tener mayúsculas, minúsculas, guiones y espacios")]
        public string? Name { get; set; }
    }
}
