using System.Text.Json.Serialization;

namespace LibraryManagementSystem.Entities.Models.University
{
    public class Student
    {
        [JsonPropertyName("StudentId")]
        public int StudentId { get; set; }

        [JsonPropertyName("FirstName")]
        public string? FirstName { get; set; }

        [JsonPropertyName("SecondName")]
        public string? SecondName { get; set; }

        [JsonPropertyName("FirstLastname")]
        public string? FirstLastname { get; set; }

        [JsonPropertyName("SecondLastname")]
        public string? SecondLastname { get; set; }

        [JsonPropertyName("Carnet")]
        public string? Carnet { get; set; }

        [JsonPropertyName("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("Sex")]
        public char Sex { get; set; }

        [JsonPropertyName("Email")]
        public string? Email { get; set; }

        [JsonPropertyName("Shift")]
        public string? Shift { get; set; }

        [JsonPropertyName("BorrowedBooks")]
        public short BorrowedBooks { get; set; }

        [JsonPropertyName("HasBorrowedMonograph")]
        public bool HasBorrowedMonograph { get; set; }

        [JsonPropertyName("Fine")]
        public decimal Fine { get; set; }

        [JsonPropertyName("CreatedOn")]
        public DateTime CreatedOn { get; set; }

        [JsonPropertyName("ModifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [JsonPropertyName("CareerId")]
        public byte CareerId { get; set; }
    }
}
