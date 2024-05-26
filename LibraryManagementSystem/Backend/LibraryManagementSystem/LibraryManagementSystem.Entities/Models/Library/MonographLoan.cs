using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.Library
{
    public class MonographLoan
    {
        [JsonPropertyName("MonographLoanId")]
        public int MonographLoanId { get; set; }

        [JsonPropertyName("StudentId")]
        public int StudentId { get; set; }

        [JsonPropertyName("MonographId")]
        public int Monographid { get; set; }

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
