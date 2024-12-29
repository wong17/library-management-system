namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class BookLoanDto : LoanDto
    {
        public int BookLoanId { get; set; }
        public string? TypeOfLoan { get; set; } // Tipo de préstamo (DOMICILIO o SALA)
        public BookDto? Book { get; set; }
    }
}