using LibraryManagementSystem.WinUI.Entities.Dtos.University;
using System.Text.Json.Serialization;

namespace LibraryManagementSystem.WinUI.Entities.Dtos.Library;

public class MonographLoanDto
{
    [JsonPropertyName("MonographLoanId")]
    public int MonographLoanId { get; set; }

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

    // Monograph
    [JsonPropertyName("Monograph")]
    public MonographDto? Monograph { get; set; }
}
