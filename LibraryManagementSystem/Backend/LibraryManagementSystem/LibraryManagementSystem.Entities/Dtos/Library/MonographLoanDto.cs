namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class MonographLoanDto : LoanDto
    {
        public int MonographLoanId { get; set; }
        public MonographDto? Monograph { get; set; }
    }
}