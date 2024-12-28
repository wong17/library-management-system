using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Library
{
    public interface IAuthorRepository : IRepositoryBase<Author>, IRepositoryCreateUpdateMany<Author>, IRepositoryGetAllBase<Author>
    {
    }
}