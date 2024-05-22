using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models.Security;

namespace LibraryManagementSystem.Bll.Interfaces.Security
{
    public interface IRoleBll : IBaseBll
    {
        Task<ApiResponse> Create(Role entity);
        Task<ApiResponse> Update(Role entity);
        Task<ApiResponse> Delete(int id);
        Task<ApiResponse> GetAll();
        Task<ApiResponse> GetById(int id);
    }
}
