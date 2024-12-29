namespace LibraryManagementSystem.Entities.Models.Library
{
    public abstract class Loan
    {
        public int StudentId { get; set; }
        public string? State { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public int BorrowedUserId { get; set; }
        public int ReturnedUserId { get; set; }
    }
}