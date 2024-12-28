using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface IBookAuthorBll : IBaseBll
    {
        Task<ApiResponse> Create(BookAuthorInsertDto entity);

        Task<ApiResponse> Delete(int bookId, int authorId);

        Task<ApiResponse> CreateMany(IEnumerable<BookAuthorInsertDto> entities);

        /* Para obtener todos los registros */

        Task<ApiResponse> GetAll();

        /* Para obtener un solo registro */

        Task<ApiResponse> GetById(int bookId, int authorId);

        Task<ApiResponse> UpdateMany(IEnumerable<BookAuthorInsertDto> entities);
    }
}