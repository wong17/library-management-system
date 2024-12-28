namespace LibraryManagementSystem.Entities.Models.Library
{
    public class BookLoan : Loan
    {
        private int bookLoanId;
        private int bookId;
        private string? typeOfLoan;

        public int BookLoanId { get => bookLoanId; set => bookLoanId = value; }
        public int BookId { get => bookId; set => bookId = value; }
        public string? TypeOfLoan { get => typeOfLoan; set => typeOfLoan = value; }
    }
}