using LibraryManagementSystem.Bll.Interfaces;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Bll.Implements
{
    public class PublisherBll(IPublisherRepository repository) : IPublisherBll
    {
        private readonly IPublisherRepository _repository = repository;

        public async Task<ApiResponse> Create(Publisher entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<Publisher> entities) => await _repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll() => await _repository.GetAll();

        public async Task<ApiResponse> GetById(int id) => await _repository.GetById(id);

        public async Task<ApiResponse> Update(Publisher entity) => await _repository.Update(entity);

        public async Task<ApiResponse> UpdateMany(IEnumerable<Publisher> entities) => await _repository.UpdateMany(entities);
    }
}
