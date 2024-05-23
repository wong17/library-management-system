using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Library
{
    public interface IMonographLoanRepository : IRepositoryLoanBase<MonographLoan>, IRepositoryGetAllBase<MonographLoan>
    {
        /* Para aprobar una solicitud de préstamo de monografía, estado de la solicitud: CREADA -> PRESTADA */
        Task<ApiResponse> UpdateBorrowedMonographLoan(int monographLoanId, DateTime dueDate);
        /* Para hacer la devolución de una monografía, estado de la solicitud: PRESTADA -> DEVUELTA */
        Task<ApiResponse> UpdateReturnedMonographLoan(int monographLoanId);
    }
}
