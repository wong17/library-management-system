using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Security;

namespace LibraryManagementSystem.Bll.Interfaces.Security
{
    public interface IUserBll : IBaseBll
    {
        Task<ApiResponse> Create(UserInsertDto entity);

        Task<ApiResponse> Update(UserUpdateDto entity);

        Task<ApiResponse> Delete(int id);

        Task<ApiResponse> GetAll();

        Task<ApiResponse> GetById(int id);

        Task<ApiResponse> LogIn(UserLogInDto entity);
    }
}