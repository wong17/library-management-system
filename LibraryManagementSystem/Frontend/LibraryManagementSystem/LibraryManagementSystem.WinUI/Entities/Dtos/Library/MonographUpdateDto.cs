using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.WinUi.Entities.Dtos.Library;

public class MonographUpdateDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Id de la monografía debe ser mayor que 0")]
    public int MonographId
    {
        get; set;
    }

    [Required]
    [StringLength(maximumLength: 25, ErrorMessage = "Clasificación de la monografía debe tener entre 1 y 25 caracteres", MinimumLength = 1)]
    [RegularExpression(@"^[a-zA-Z0-9\-. ]+$", ErrorMessage = "Clasificación de la monografía solo puede tener mayúsculas, minúsculas, guiones, puntos y espacios")]
    public string? Classification
    {
        get; set;
    }

    [Required]
    [StringLength(maximumLength: 250, ErrorMessage = "Titulo de la monografía debe tener entre 1 y 250 caracteres", MinimumLength = 1)]
    [RegularExpression(@"^[a-zA-Z0-9\-. ]+$", ErrorMessage = "Titulo de la monografia solo puede tener mayúsculas, minúsculas, números, guiones, puntos y espacios")]
    public string? Title
    {
        get; set;
    }

    [StringLength(maximumLength: 500, ErrorMessage = "Descripción de la monografía debe tener un máximo de 500 caracteres")]
    public string? Description
    {
        get; set;
    }

    [Required]
    [StringLength(maximumLength: 100, ErrorMessage = "Tutor de la monografía debe tener un máximo de 100 caracteres")]
    [RegularExpression(@"^[a-zA-Z. ]+$", ErrorMessage = "Tutor de la monografía solo puede tener mayúsculas, minúsculas puntos y espacios")]
    public string? Tutor
    {
        get; set;
    }

    [Required]
    public DateTime PresentationDate
    {
        get; set;
    }

    public byte[]? Image
    {
        get; set;
    }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Id de la carrera debe ser mayor que 0")]
    public int CareerId
    {
        get; set;
    }

    [Required]
    public bool IsAvailable
    {
        get; set;
    }

    [Required]
    public bool IsActive
    {
        get; set;
    }
}