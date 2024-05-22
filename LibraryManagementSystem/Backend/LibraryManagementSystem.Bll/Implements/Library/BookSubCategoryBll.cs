using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class BookSubCategoryBll(IBookSubCategoryRepository repository) : IBookSubCategoryBll
    {
        private readonly IBookSubCategoryRepository _repository = repository;

        public async Task<ApiResponse> Create(BookSubCategory entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<BookSubCategory> entities) => await _repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int bookId, int subCategoryId) => await _repository.Delete(bookId, subCategoryId);

        public async Task<ApiResponse> GetAll() => await _repository.GetAll();

        public async Task<ApiResponse> GetById(int bookId, int subCategoryId) => await _repository.GetById(bookId, subCategoryId);
    }
}
