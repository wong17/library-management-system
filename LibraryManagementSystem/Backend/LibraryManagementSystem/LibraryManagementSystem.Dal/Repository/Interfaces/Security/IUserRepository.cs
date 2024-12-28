using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models.Security;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Security
{
    public interface IUserRepository : IRepositoryBase<User>, IRepositoryGetAllBase<User>
    {
        Task<ApiResponse> LogIn(User entity);
    }
}