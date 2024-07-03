namespace LibraryManagementSystem.Entities.Models.Library
{
    public class MonographLoan : Loan
    {
        private int monographLoanId;
        private int monographId;

        public int MonographLoanId { get => monographLoanId; set => monographLoanId = value; }
        public int MonographId { get => monographId; set => monographId = value; }
    }
}
