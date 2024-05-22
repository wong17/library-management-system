using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class BookAuthorBll(IBookAuthorRepository repository) : IBookAuthorBll
    {
        private readonly IBookAuthorRepository _repository = repository;

        public async Task<ApiResponse> Create(BookAuthor entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<BookAuthor> entities) => await _repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int bookId, int authorId) => await _repository.Delete(bookId, authorId);

        public async Task<ApiResponse> GetAll() => await _repository.GetAll();

        public async Task<ApiResponse> GetById(int bookId, int authorId) => await _repository.GetById(bookId, authorId);
    }
}
