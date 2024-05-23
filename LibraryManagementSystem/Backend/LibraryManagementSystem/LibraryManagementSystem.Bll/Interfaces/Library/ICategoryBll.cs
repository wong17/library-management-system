using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface ICategoryBll : IBaseBll
    {
        Task<ApiResponse> Create(Category entity);

        Task<ApiResponse> Update(Category entity);

        Task<ApiResponse> Delete(int id);
        /* Para insertar varios registros a la vez */
        Task<ApiResponse> CreateMany(IEnumerable<Category> entities);
        /* Para actualizar varios registros a la vez */
        Task<ApiResponse> UpdateMany(IEnumerable<Category> entities);
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int id);
    }
}
