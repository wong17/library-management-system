using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models.Security;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Security
{
    public interface IRoleRepository : IRepositoryBase<Role>, IRepositoryGetAllBase<Role>
    {
    }
}
