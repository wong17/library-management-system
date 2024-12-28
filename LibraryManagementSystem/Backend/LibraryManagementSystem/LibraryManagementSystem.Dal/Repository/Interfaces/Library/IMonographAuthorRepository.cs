using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Library
{
    public interface IMonographAuthorRepository : IRepositoryManyToManyBase<MonographAuthor>
    {
        Task<ApiResponse> UpdateMany(IEnumerable<MonographAuthor> entities);
    }
}