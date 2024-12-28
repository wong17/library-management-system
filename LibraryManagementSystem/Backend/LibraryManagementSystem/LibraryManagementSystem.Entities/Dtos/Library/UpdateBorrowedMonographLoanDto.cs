using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class UpdateBorrowedMonographLoanDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del préstamo de monografía debe ser mayor que 0")]
        public int MonographLoanId { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del usuario que aprobó el préstamo debe ser mayor que 0")]
        public int BorrowedUserId { get; set; }
    }
}