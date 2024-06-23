using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class UpdateReturnedBookLoanDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del préstamo de libro debe ser mayor que 0")]
        public int BookLoanId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Id del usuario que recibió el libro debe ser mayor que 0")]
        public int ReturnedUserId { get; set; }
    }
}
