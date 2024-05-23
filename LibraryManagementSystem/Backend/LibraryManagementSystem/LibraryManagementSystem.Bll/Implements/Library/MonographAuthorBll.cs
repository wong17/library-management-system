using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class MonographAuthorBll(IMonographAuthorRepository repository) : IMonographAuthorBll
    {
        private readonly IMonographAuthorRepository _repository = repository;

        public async Task<ApiResponse> Create(MonographAuthor entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<MonographAuthor> entities) => await _repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int monographId, int authorId) => await _repository.Delete(monographId, authorId);

        public async Task<ApiResponse> GetAll() => await _repository.GetAll();

        public async Task<ApiResponse> GetById(int monographId, int authorId) => await _repository.GetById(monographId, authorId);
    }
}
