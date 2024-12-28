using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface IAuthorBll : IBaseBll
    {
        Task<ApiResponse> Create(AuthorInsertDto entity);

        Task<ApiResponse> Update(AuthorUpdateDto entity);

        Task<ApiResponse> Delete(int id);

        /* Para insertar varios registros a la vez */

        Task<ApiResponse> CreateMany(IEnumerable<AuthorInsertDto> entities);

        /* Para actualizar varios registros a la vez */

        Task<ApiResponse> UpdateMany(IEnumerable<AuthorUpdateDto> entities);

        /* Para obtener todos los registros */

        Task<ApiResponse> GetAll();

        /* Para obtener un solo registro */

        Task<ApiResponse> GetById(int id);
    }
}