namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class UpdateBorrowedMonographLoanDto
    {
        public int MonographLoanId { get; set; }
        public DateTime DueDate { get; set; }
        public int BorrowedUserId { get; set; }
    }
}
