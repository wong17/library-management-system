using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class CategoryBll(ICategoryRepository repository) : ICategoryBll
    {
        private readonly ICategoryRepository _repository = repository;

        public async Task<ApiResponse> Create(Category entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<Category> entities) => await _repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll() => await _repository.GetAll();

        public async Task<ApiResponse> GetById(int id) => await _repository.GetById(id);

        public async Task<ApiResponse> Update(Category entity) => await _repository.Update(entity);

        public async Task<ApiResponse> UpdateMany(IEnumerable<Category> entities) => await _repository.UpdateMany(entities);
    }
}
