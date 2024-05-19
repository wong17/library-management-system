using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Bll.Interfaces
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
