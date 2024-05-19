using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos
{
    public class BookInsertDto
    {
        [Required]
        [StringLength(maximumLength: 13, ErrorMessage = "ISBN10 del libro debe tener un máximo de 13 caracteres incluyendo guiones", MinimumLength = 10)]
        public string? ISBN10 { get; set; }

        [Required]
        [StringLength(maximumLength: 17, ErrorMessage = "ISBN13 del libro debe tener un máximo de 17 caracteres incluyendo guiones", MinimumLength = 14)]
        public string? ISBN13 { get; set; }

        [Required]
        [StringLength(maximumLength: 25, ErrorMessage = "Clasificación del libro debe tener entre 1 y 25 caracteres", MinimumLength = 1)]
        public string? Classification { get; set; }

        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Titulo del libro debe tener entre 1 y 100 caracteres", MinimumLength = 1)]
        public string? Title { get; set; }

        [StringLength(maximumLength: 500, ErrorMessage = "Descripción del libro debe tener un máximo de 500 caracteres")]
        public string? Description { get; set; }

        [Required]
        [Range(1900, 9999, ErrorMessage = "Año de publicación del libro fuera de rango 1900 - año actual")]
        public short PublicationYear { get; set; }

        public byte[]? Image { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id de la editorial debe ser mayor que 0")]
        public int PublisherId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id de la categoría debe ser mayor que 0")]
        public int CategoryId { get; set; }

        [Required]
        [Range(0, short.MaxValue, ErrorMessage = "Cantidad de libros fuera de rango 0 - 32,767")]
        public short NumberOfCopies { get; set; }

        [Required]
        public bool IsAvailable { get; set; }
    }
}
