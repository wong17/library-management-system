using LibraryManagementSystem.Entities.Dtos.Security;
using LibraryManagementSystem.Entities.Dtos.University;

namespace LibraryManagementSystem.Entities.Dtos.Library
{
    public abstract class LoanDto
    {
        // Estado de la solicitud
        public string? State { get; set; }

        // Fecha que se solicita el préstamo ya sea de un libro o una monografía
        public DateTime LoanDate { get; set; }

        // Fecha tope para entregar el libro o monografía que se prestó
        public DateTime DueDate { get; set; }

        // Fecha que el estudiante devolvió el libro o monografía
        public DateTime ReturnDate { get; set; }

        // Student
        public StudentDto? Student { get; set; }

        // BorrowedUser, persona que aprobó el préstamo
        public UserDto? BorrowedUser { get; set; }

        // ReturnedUser, persona que recibió el libro o monografía prestado
        public UserDto? ReturnedUser { get; set; }
    }
}