namespace LibraryManagementSystem.Entities.Models.Library
{
    public class MonographLoan
    {
        public int MonographLoanId { get; set; }
        public int StudentId { get; set; }
        public int MonographId { get; set; }
        public string? State { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int BorrowedUserId { get; set; }
        public int ReturnedUserId { get; set; }
    }
}
