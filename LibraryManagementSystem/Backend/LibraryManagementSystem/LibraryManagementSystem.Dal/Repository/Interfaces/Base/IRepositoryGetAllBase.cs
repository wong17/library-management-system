using LibraryManagementSystem.Common.Runtime;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Base
{
    public interface IRepositoryGetAllBase<in T> where T : class
    {
        /* Para obtener todos los registros */

        Task<ApiResponse> GetAll();

        /* Para obtener un solo registro */

        Task<ApiResponse> GetById(int id);
    }
}