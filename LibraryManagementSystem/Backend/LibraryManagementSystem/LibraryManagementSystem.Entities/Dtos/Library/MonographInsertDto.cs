using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class MonographInsertDto
    {
        [Required]
        [StringLength(maximumLength: 25, ErrorMessage = "Clasificación de la monografía debe tener entre 1 y 25 caracteres", MinimumLength = 1)]
        [RegularExpression(@"^[\p{L}0-9\-\. ]+$", ErrorMessage = "Clasificación de la monografía solo puede tener letras, guiones, puntos y espacios")]
        public string? Classification { get; set; }

        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "Titulo de la monografía debe tener entre 1 y 250 caracteres", MinimumLength = 1)]
        [RegularExpression(@"^[\p{L}0-9\-\.\, ]+$", ErrorMessage = "Titulo de la monografía solo puede tener letras, números, guiones, puntos, comas y espacios")]
        public string? Title { get; set; }

        [StringLength(maximumLength: 500, ErrorMessage = "Descripción de la monografía debe tener un máximo de 500 caracteres")]
        public string? Description { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Tutor de la monografía debe tener un máximo de 100 caracteres", MinimumLength = 1)]
        [RegularExpression(@"^[\p{L}\.\. ]+$", ErrorMessage = "Tutor de la monografía solo puede tener letras, puntos, comas y espacios")]
        public string? Tutor { get; set; }

        [Required]
        public DateTime PresentationDate { get; set; }

        public string? Image { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id de la carrera debe ser mayor que 0")]
        public int CareerId { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}