using LibraryManagementSystem.Bll.Interfaces;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Bll.Implements
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
