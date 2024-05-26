using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.WinUI.Entities.Dtos.Security;

public class UserInsertDto
{
    [Required]
    [StringLength(maximumLength: 50, ErrorMessage = "Nombre de usuario debe tener entre 1 y 50 caracteres", MinimumLength = 1)]
    public string? UserName
    {
        get; set;
    }

    [Required]
    [StringLength(maximumLength: 250, ErrorMessage = "Correo electronico debe tener entre 1 y 250 caracteres", MinimumLength = 1)]
    public string? Email
    {
        get; set;
    }

    [Required]
    [StringLength(maximumLength: 64, ErrorMessage = "Contraseña debe tener entre 1 y 64 caracteres", MinimumLength = 1)]
    public string? Password
    {
        get; set;
    }
}