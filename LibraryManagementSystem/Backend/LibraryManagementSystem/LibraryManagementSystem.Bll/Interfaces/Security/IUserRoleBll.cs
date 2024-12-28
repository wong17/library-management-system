using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Security;

namespace LibraryManagementSystem.Bll.Interfaces.Security
{
    public interface IUserRoleBll : IBaseBll
    {
        Task<ApiResponse> Create(UserRoleInsertDto entity);

        Task<ApiResponse> Delete(int userId, int roleId);

        Task<ApiResponse> CreateMany(IEnumerable<UserRoleInsertDto> entities);

        Task<ApiResponse> GetAll();

        Task<ApiResponse> GetById(int userId, int roleId);
    }
}