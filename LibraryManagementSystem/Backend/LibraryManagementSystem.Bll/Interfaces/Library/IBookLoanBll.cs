using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface IBookLoanBll : IBaseBll
    {
        Task<ApiResponse> Create(BookLoan entity);
        
        Task<ApiResponse> Delete(int id);
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
