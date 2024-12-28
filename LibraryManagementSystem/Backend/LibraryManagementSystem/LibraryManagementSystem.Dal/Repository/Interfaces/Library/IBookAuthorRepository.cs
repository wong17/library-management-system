using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Library
{
    public interface IBookAuthorRepository : IRepositoryManyToManyBase<BookAuthor>
    {
        Task<ApiResponse> UpdateMany(IEnumerable<BookAuthor> entities);
    }
}