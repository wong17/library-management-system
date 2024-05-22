using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models.Security;

namespace LibraryManagementSystem.Bll.Interfaces.Security
{
    public interface IUserRoleBll : IBaseBll
    {
        Task<ApiResponse> Create(UserRole entity);
        Task<ApiResponse> Delete(int userId, int roleId);
        Task<ApiResponse> CreateMany(IEnumerable<UserRole> entities);
        Task<ApiResponse> GetAll();
        Task<ApiResponse> GetById(int userId, int roleId);
    }
}
