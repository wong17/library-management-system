namespace LibraryManagementSystem.WinUI.Entities.Models.Library;

public class Loan
{
    public int StudentId { get; set; }
    public string? State { get; set; }
    public DateTime LoanDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime ReturnDate { get; set; }
}