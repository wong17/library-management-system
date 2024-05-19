namespace LibraryManagementSystem.Entities.Models
{
    public class MonographLoan
    {
        public int MonographLoanId { get; set; }
        public int StudentId { get; set; }
        public int Monographid { get; set; }
        public string? State { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
