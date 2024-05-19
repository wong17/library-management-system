using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Dal.Repository.Interfaces
{
    public interface IBookLoanRepository : IRepositoryLoanBase<BookLoan>
    {
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int id);
        /* Para aprobar una solicitud de préstamo de libro, estado de la solicitud: CREADA -> PRESTADO */
        Task<ApiResponse> UpdateBorrowedBookLoan(int bookLoanId, DateTime dueDate);
        /* Para hacer la devolución de un libro, estado de la solicitud: PRESTADO -> DEVUELTO */
        Task<ApiResponse> UpdateReturnedBookLoan(int bookLoanId);
    }
}
