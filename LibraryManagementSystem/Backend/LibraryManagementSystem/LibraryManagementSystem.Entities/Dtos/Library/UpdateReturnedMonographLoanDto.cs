using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class UpdateReturnedMonographLoanDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del préstamo de monografía debe ser mayor que 0")]
        public int MonographLoanId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del usuario que recibió el libro debe ser mayor que 0")]
        public int ReturnedUserId { get; set; }
    }
}