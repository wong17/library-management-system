using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models.Security;

namespace LibraryManagementSystem.Bll.Interfaces.Security
{
    public interface IUserBll : IBaseBll
    {
        Task<ApiResponse> Create(User entity);
        Task<ApiResponse> Update(User entity);
        Task<ApiResponse> Delete(int id);
        Task<ApiResponse> GetAll();
        Task<ApiResponse> GetById(int id);
    }
}
