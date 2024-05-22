using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Dal.Repository.Interfaces
{
    public interface IPublisherRepository : IRepositoryBase<Publisher>, IRepositoryCreateUpdateMany<Publisher>
    {
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int id);
    }
}
