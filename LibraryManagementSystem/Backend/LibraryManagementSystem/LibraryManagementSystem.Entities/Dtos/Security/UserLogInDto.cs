using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Security
{
    public class UserLogInDto
    {
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Nombre de usuario debe tener entre 1 y 50 caracteres", MinimumLength = 1)]
        [RegularExpression(@"^[\p{L}0-9\. ]+$", ErrorMessage = "Nombre de usuario solo puede tener letras, números y puntos")]
        public string? UserName { get; set; }

        [Required]
        [StringLength(maximumLength: 64, ErrorMessage = "Contraseña debe tener entre 1 y 64 caracteres", MinimumLength = 1)]
        public string? Password { get; set; }
    }
}
