using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface ICategoryBll : IBaseBll
    {
        Task<ApiResponse> Create(CategoryInsertDto entity);

        Task<ApiResponse> Update(CategoryUpdateDto entity);

        Task<ApiResponse> Delete(int id);

        /* Para insertar varios registros a la vez */

        Task<ApiResponse> CreateMany(IEnumerable<CategoryInsertDto> entities);

        /* Para actualizar varios registros a la vez */

        Task<ApiResponse> UpdateMany(IEnumerable<CategoryUpdateDto> entities);

        /* Para obtener todos los registros */

        Task<ApiResponse> GetAll();

        /* Para obtener un solo registro */

        Task<ApiResponse> GetById(int id);
    }
}