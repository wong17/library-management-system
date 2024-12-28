namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class MonographLoanDto : LoanDto
    {
        private int monographLoanId;
        private MonographDto? monograph;

        public int MonographLoanId { get => monographLoanId; set => monographLoanId = value; }

        // Monograph
        public MonographDto? Monograph { get => monograph; set => monograph = value; }
    }
}