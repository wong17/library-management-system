using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface IBookBll : IBaseBll
    {
        Task<ApiResponse> Create(BookInsertDto entity);

        Task<ApiResponse> Update(BookUpdateDto entity);

        Task<ApiResponse> Delete(int id);

        /* Para obtener todos los registros */

        Task<ApiResponse> GetAll();

        /* Para obtener un solo registro */

        Task<ApiResponse> GetById(int id);

        Task<ApiResponse> GetFilteredBook(FilterBookDto filterBookDto);
    }
}