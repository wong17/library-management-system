using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Bll.Interfaces
{
    public interface IBookBll : IBaseBll
    {
        Task<ApiResponse> Create(Book entity);

        Task<ApiResponse> Update(Book entity);
        
        Task<ApiResponse> Delete(int id);
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int id);
    }
}
