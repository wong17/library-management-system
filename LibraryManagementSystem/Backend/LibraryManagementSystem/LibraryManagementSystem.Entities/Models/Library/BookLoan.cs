using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.Library
{
    public class BookLoan
    {
        [JsonPropertyName("BookLoanId")]
        public int BookLoanId { get; set; }

        [JsonPropertyName("StudentId")]
        public int StudentId { get; set; }

        [JsonPropertyName("BookId")]
        public int BookId { get; set; }

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
    }
}
