using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface IMonographLoanBll : IBaseBll
    {
        Task<ApiResponse> Create(MonographLoan entity);

        Task<ApiResponse> Delete(int id);
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int id);
        /* Para aprobar una solicitud de préstamo de monografía, estado de la solicitud: CREADA -> PRESTADA */
        Task<ApiResponse> UpdateBorrowedMonographLoan(UpdateBorrowedMonographLoanDto updateBorrowedMonographLoanDto);
        /* Para hacer la devolución de una monografía, estado de la solicitud: PRESTADA -> DEVUELTA */
        Task<ApiResponse> UpdateReturnedMonographLoan(UpdateReturnedMonographLoanDto updateReturnedMonographLoanDto);
    }
}
