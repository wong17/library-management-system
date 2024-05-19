using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Bll.Interfaces
{
    public interface IAuthorBll : IBaseBll
    {
        Task<ApiResponse> Create(Author entity);

        Task<ApiResponse> Update(Author entity);

        Task<ApiResponse> Delete(int id);
        /* Para insertar varios registros a la vez */
        Task<ApiResponse> CreateMany(IEnumerable<Author> entities);
        /* Para actualizar varios registros a la vez */
        Task<ApiResponse> UpdateMany(IEnumerable<Author> entities);
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int id);
    }
}
