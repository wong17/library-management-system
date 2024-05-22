using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Bll.Interfaces
{
    public interface IMonographAuthorBll : IBaseBll
    {
        Task<ApiResponse> Create(MonographAuthor entity);

        Task<ApiResponse> Delete(int monographId, int authorId);

        Task<ApiResponse> CreateMany(IEnumerable<MonographAuthor> entities);
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int monographId, int authorId);
    }
}
