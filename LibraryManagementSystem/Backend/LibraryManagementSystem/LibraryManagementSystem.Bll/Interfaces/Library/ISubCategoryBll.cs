using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface ISubCategoryBll : IBaseBll
    {
        Task<ApiResponse> Create(SubCategoryInsertDto entity);

        Task<ApiResponse> Update(SubCategoryUpdateDto entity);

        Task<ApiResponse> Delete(int id);

        /* Para insertar varios registros a la vez */

        Task<ApiResponse> CreateMany(IEnumerable<SubCategoryInsertDto> entities);

        /* Para actualizar varios registros a la vez */

        Task<ApiResponse> UpdateMany(IEnumerable<SubCategoryUpdateDto> entities);

        /* Para obtener todos los registros */

        Task<ApiResponse> GetAll();

        /* Para obtener un solo registro */

        Task<ApiResponse> GetById(int id);
    }
}