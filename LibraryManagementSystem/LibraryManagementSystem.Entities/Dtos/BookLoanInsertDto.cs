using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos
{
    public class BookLoanInsertDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del estudiante debe ser mayor que 0")]
        public int StudentId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del libro debe ser mayor que 0")]
        public int BookId { get; set; }

        [Required]
        [StringLength(maximumLength: 10, ErrorMessage = "Tipo de préstamo del libro debe ser SALA o DOMICILIO", MinimumLength = 4)]
        public string? TypeOfLoan { get; set; }
    }
}
