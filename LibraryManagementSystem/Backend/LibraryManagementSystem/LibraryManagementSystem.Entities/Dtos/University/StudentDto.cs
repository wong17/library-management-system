namespace LibraryManagementSystem.Entities.Dtos.University
{
    public class StudentDto
    {
        public int StudentId { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? FirstLastname { get; set; }
        public string? SecondLastname { get; set; }
        public string? Carnet { get; set; }
        public string? PhoneNumber { get; set; }
        public char Sex { get; set; }
        public string? Email { get; set; }
        public string? Shift { get; set; }
        public short BorrowedBooks { get; set; }
        public bool HasBorrowedMonograph { get; set; }
        public decimal Fine { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        // Career
        public CareerDto? Career { get; set; }
    }
}
