using LibraryManagementSystem.Bll.Interfaces;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Bll.Implements
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
