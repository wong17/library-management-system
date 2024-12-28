namespace LibraryManagementSystem.Entities.Models.University
{
    public class Student
    {
        private int studentId;
        private string? firstName;
        private string? secondName;
        private string? firstLastname;
        private string? secondLastname;
        private string? carnet;
        private string? phoneNumber;
        private char sex;
        private string? email;
        private string? shift;
        private short borrowedBooks;
        private bool hasBorrowedMonograph;
        private decimal fine;
        private DateTime createdOn;
        private DateTime modifiedOn;
        private byte careerId;

        public int StudentId { get => studentId; set => studentId = value; }
        public string? FirstName { get => firstName; set => firstName = value; }
        public string? SecondName { get => secondName; set => secondName = value; }
        public string? FirstLastname { get => firstLastname; set => firstLastname = value; }
        public string? SecondLastname { get => secondLastname; set => secondLastname = value; }
        public string? Carnet { get => carnet; set => carnet = value; }
        public string? PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public char Sex { get => sex; set => sex = value; }
        public string? Email { get => email; set => email = value; }
        public string? Shift { get => shift; set => shift = value; }
        public short BorrowedBooks { get => borrowedBooks; set => borrowedBooks = value; }
        public bool HasBorrowedMonograph { get => hasBorrowedMonograph; set => hasBorrowedMonograph = value; }
        public decimal Fine { get => fine; set => fine = value; }
        public DateTime CreatedOn { get => createdOn; set => createdOn = value; }
        public DateTime ModifiedOn { get => modifiedOn; set => modifiedOn = value; }
        public byte CareerId { get => careerId; set => careerId = value; }
    }
}