using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class AuthorBll(IAuthorRepository repository) : IAuthorBll
    {
        private readonly IAuthorRepository _repository = repository;

        public async Task<ApiResponse> Create(Author entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<Author> entities) => await _repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll() => await _repository.GetAll();

        public async Task<ApiResponse> GetById(int id) => await _repository.GetById(id);

        public async Task<ApiResponse> Update(Author entity) => await _repository.Update(entity);

        public async Task<ApiResponse> UpdateMany(IEnumerable<Author> entities) => await _repository.UpdateMany(entities);
    }
}
