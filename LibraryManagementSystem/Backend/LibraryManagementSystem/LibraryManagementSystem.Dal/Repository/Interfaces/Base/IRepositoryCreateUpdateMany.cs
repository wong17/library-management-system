using LibraryManagementSystem.Common.Runtime;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Base
{
    public interface IRepositoryCreateUpdateMany<in T> where T : class
    {
        /* Para insertar varios registros a la vez */

        Task<ApiResponse> CreateMany(IEnumerable<T> entities);

        /* Para actualizar varios registros a la vez */

        Task<ApiResponse> UpdateMany(IEnumerable<T> entities);
    }
}