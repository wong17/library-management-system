using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models.Security;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Security
{
    public interface IUserRoleRepository : IRepositoryManyToManyBase<UserRole>
    {
    }
}