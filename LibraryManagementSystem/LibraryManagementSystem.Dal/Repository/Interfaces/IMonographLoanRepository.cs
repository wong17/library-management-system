using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Dal.Repository.Interfaces
{
    public interface IMonographLoanRepository : IRepositoryLoanBase<MonographLoan>
    {
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int id);
        /* Para aprobar una solicitud de préstamo de monografía, estado de la solicitud: CREADA -> PRESTADA */
        Task<ApiResponse> UpdateBorrowedMonographLoan(int monographLoanId, DateTime dueDate);
        /* Para hacer la devolución de una monografía, estado de la solicitud: PRESTADA -> DEVUELTA */
        Task<ApiResponse> UpdateReturnedMonographLoan(int monographLoanId);
    }
}
