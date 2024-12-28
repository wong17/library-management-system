using LibraryManagementSystem.Entities.Dtos.Security;
using LibraryManagementSystem.Entities.Dtos.University;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public abstract class LoanDto
    {
        protected DateTime loanDate;
        protected DateTime dueDate;
        protected DateTime returnDate;
        protected UserDto? borrowedUser;
        protected UserDto? returnedUser;
        protected StudentDto? student;
        protected string? state;

        // Estado de la solicitud
        public string? State { get => state; set => state = value; }

        // Fecha que se solicita el préstamo ya sea de un libro o una monografía
        public DateTime LoanDate { get => loanDate; set => loanDate = value; }

        // Fecha tope para entregar el libro o monografía que se prestó
        public DateTime DueDate { get => dueDate; set => dueDate = value; }

        // Fecha que el estudiante devolvio el libro o monografía
        public DateTime ReturnDate { get => returnDate; set => returnDate = value; }

        // Student
        public StudentDto? Student { get => student; set => student = value; }

        // BorrowedUser, persona que aprobó el préstamo
        public UserDto? BorrowedUser { get => borrowedUser; set => borrowedUser = value; }

        // ReturnedUser, persona que recibió el libro o monografía prestado
        public UserDto? ReturnedUser { get => returnedUser; set => returnedUser = value; }
    }
}