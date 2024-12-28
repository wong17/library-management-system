using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Security;

namespace LibraryManagementSystem.Bll.Interfaces.Security
{
    public interface IRoleBll : IBaseBll
    {
        Task<ApiResponse> Create(RoleInsertDto entity);

        Task<ApiResponse> Update(RoleUpdateDto entity);

        Task<ApiResponse> Delete(int id);

        Task<ApiResponse> GetAll();

        Task<ApiResponse> GetById(int id);
    }
}