using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Library
{
    public interface IBookSubCategoryRepository : IRepositoryManyToManyBase<BookSubCategory>
    {
        Task<ApiResponse> UpdateMany(IEnumerable<BookSubCategory> entities);
    }
}
