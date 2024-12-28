namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class BookLoanDto : LoanDto
    {
        private int bookLoanId;
        private string? typeOfLoan;
        private BookDto? book;

        public int BookLoanId { get => bookLoanId; set => bookLoanId = value; }

        // Tipo de préstamo (DOMICILIO o SALA)
        public string? TypeOfLoan { get => typeOfLoan; set => typeOfLoan = value; }

        // Book
        public BookDto? Book { get => book; set => book = value; }
    }
}