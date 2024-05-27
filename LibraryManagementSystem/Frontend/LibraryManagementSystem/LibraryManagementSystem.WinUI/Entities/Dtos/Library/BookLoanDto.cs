using LibraryManagementSystem.WinUI.Entities.Dtos.University;
using System.Text.Json.Serialization;

namespace LibraryManagementSystem.WinUI.Entities.Dtos.Library;

public class BookLoanDto
{
    [JsonPropertyName("BookLoanId")]
    public int BookLoanId { get; set; }

    [JsonPropertyName("TypeOfLoan")]
    public string? TypeOfLoan { get; set; }

    [JsonPropertyName("State")]
    public string? State { get; set; }

    [JsonPropertyName("LoanDate")]
    public DateTime LoanDate { get; set; }

    [JsonPropertyName("DueDate")]
    public DateTime DueDate { get; set; }

    [JsonPropertyName("ReturnDate")]
    public DateTime ReturnDate { get; set; }

    // Student
    [JsonPropertyName("Student")]
    public StudentDto? Student { get; set; }

    // Book
    [JsonPropertyName("Book")]
    public BookDto? Book { get; set; }
}
