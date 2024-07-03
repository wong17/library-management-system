namespace LibraryManagementSystem.Entities.Models.Library
{
    public abstract class Loan
    {
        protected int studentId;
        protected string? state;
        protected DateTime loanDate;
        protected DateTime dueDate;
        protected DateTime returnDate;
        protected int borrowedUserId;
        protected int returnedUserId;

        public int StudentId { get => studentId; set => studentId = value; }
        public string? State { get => state; set => state = value; }
        public DateTime LoanDate { get => loanDate; set => loanDate = value; }
        public DateTime DueDate { get => dueDate; set => dueDate = value; }
        public DateTime ReturnDate { get => returnDate; set => returnDate = value; }
        public int BorrowedUserId { get => borrowedUserId; set => borrowedUserId = value; }
        public int ReturnedUserId { get => returnedUserId; set => returnedUserId = value; }
    }
}
