using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Bll.Interfaces
{
    public interface IPublisherBll : IBaseBll
    {
        Task<ApiResponse> Create(Publisher entity);

        Task<ApiResponse> Update(Publisher entity);
        
        Task<ApiResponse> Delete(int id);
        /* Para insertar varios registros a la vez */
        Task<ApiResponse> CreateMany(IEnumerable<Publisher> entities);
        /* Para actualizar varios registros a la vez */
        Task<ApiResponse> UpdateMany(IEnumerable<Publisher> entities);
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int id);
    }
}
