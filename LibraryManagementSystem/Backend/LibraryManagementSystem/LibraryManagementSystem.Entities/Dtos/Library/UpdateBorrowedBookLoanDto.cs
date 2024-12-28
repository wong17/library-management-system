using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class UpdateBorrowedBookLoanDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del préstamo de libro debe ser mayor que 0")]
        public int BookLoanId { get; set; }

        [Required]
        public DateTime DueDate { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del usuario que aprobó el préstamo debe ser mayor que 0")]
        public int BorrowedUserId { get; set; }
    }
}