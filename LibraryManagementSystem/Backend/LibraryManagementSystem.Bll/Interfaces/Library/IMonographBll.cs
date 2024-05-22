using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface IMonographBll : IBaseBll
    {
        Task<ApiResponse> Create(Monograph entity);

        Task<ApiResponse> Update(Monograph entity);
        
        Task<ApiResponse> Delete(int id);
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int id);
    }
}
