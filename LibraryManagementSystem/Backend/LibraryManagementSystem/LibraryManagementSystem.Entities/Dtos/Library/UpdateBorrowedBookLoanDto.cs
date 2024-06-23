namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class UpdateBorrowedBookLoanDto
    {
        public int BookLoanId { get; set; }
        public DateTime DueDate { get; set; }
        public int BorrowedUserId { get; set; }
    }
}
