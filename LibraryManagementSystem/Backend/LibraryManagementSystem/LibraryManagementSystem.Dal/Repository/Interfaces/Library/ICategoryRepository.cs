using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Library
{
    public interface ICategoryRepository : IRepositoryBase<Category>, IRepositoryCreateUpdateMany<Category>, IRepositoryGetAllBase<Category>
    {
    }
}