using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface IBookSubCategoryBll : IBaseBll
    {
        Task<ApiResponse> Create(BookSubCategoryInsertDto entity);

        Task<ApiResponse> Delete(int bookId, int subCategoryId);

        Task<ApiResponse> CreateMany(IEnumerable<BookSubCategoryInsertDto> entities);

        /* Para obtener todos los registros */

        Task<ApiResponse> GetAll();

        /* Para obtener un solo registro */

        Task<ApiResponse> GetById(int bookId, int subCategoryId);

        Task<ApiResponse> UpdateMany(IEnumerable<BookSubCategoryInsertDto> entities);
    }
}