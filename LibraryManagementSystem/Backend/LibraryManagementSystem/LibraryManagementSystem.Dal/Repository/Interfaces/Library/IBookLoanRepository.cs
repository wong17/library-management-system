using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Library
{
    public interface IBookLoanRepository : IRepositoryLoanBase<BookLoan>, IRepositoryGetAllBase<BookLoan>
    {
        /* Para aprobar una solicitud de préstamo de libro, estado de la solicitud: CREADA -> PRESTADO */
        Task<ApiResponse> UpdateBorrowedBookLoan(UpdateBorrowedBookLoanDto updateBorrowedBookLoanDto);
        /* Para hacer la devolución de un libro, estado de la solicitud: PRESTADO -> DEVUELTO */
        Task<ApiResponse> UpdateReturnedBookLoan(UpdateReturnedBookLoanDto updateReturnedBookLoanDto);
    }
}
