namespace LibraryManagementSystem.Entities.Models
{
    public class BookLoan
    {
        public int BookLoanId { get; set; }
        public int StudentId { get; set; }
        public int BookId { get; set; }
        public string? TypeOfLoan { get; set; }
        public string? State { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
