namespace LibraryManagementSystem.Entities.Models.Library
{
    public class BookLoan : Loan
    {
        public int BookLoanId { get; set; }
        public int BookId { get; set; }
        public string? TypeOfLoan { get; set; }
    }
}