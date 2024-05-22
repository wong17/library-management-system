using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Security
{
    public class RoleUpdateDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del rol debe ser mayor que 0")]
        public int RoleId { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Nombre del rol debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
        public string? Name { get; set; }

        [Required]
        [StringLength(maximumLength: 500, ErrorMessage = "Descripción debe tener entre 1 y 500 caracteres", MinimumLength = 1)]
        public string? Description { get; set; }
    }
}
