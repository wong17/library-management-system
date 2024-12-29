namespace LibraryManagementSystem.Entities.Models.Library
{
    public class MonographLoan : Loan
    {
        public int MonographLoanId { get; set; }
        public int MonographId { get; set; }
    }
}