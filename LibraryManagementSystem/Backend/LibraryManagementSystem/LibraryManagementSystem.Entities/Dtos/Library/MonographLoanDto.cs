using LibraryManagementSystem.Entities.Dtos.Security;
using LibraryManagementSystem.Entities.Dtos.University;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class MonographLoanDto
    {
        public int MonographLoanId { get; set; }
        public string? State { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        // Student
        public StudentDto? Student { get; set; }
        // Monograph
        public MonographDto? Monograph { get; set; }
        // BorrowedUser
        public UserDto? BorrowedUser { get; set; }
        // ReturnedUser
        public UserDto? ReturnedUser { get; set; }
    }
}
