using LibraryManagementSystem.Entities.Dtos.Security;
using LibraryManagementSystem.Entities.Dtos.University;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public class BookLoanDto
    {
        public int BookLoanId { get; set; }
        public string? TypeOfLoan { get; set; }
        public string? State { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        // Student
        public StudentDto? Student { get; set; }
        // Book
        public BookDto? Book { get; set; }
        // BorrowedUser
        public UserDto? BorrowedUser { get; set; }
        // ReturnedUser
        public UserDto? ReturnedUser { get; set; }
    }
}
