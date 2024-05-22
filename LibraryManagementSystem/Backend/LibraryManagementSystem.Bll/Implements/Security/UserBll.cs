using LibraryManagementSystem.Bll.Interfaces.Security;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Security;
using LibraryManagementSystem.Entities.Models.Security;

namespace LibraryManagementSystem.Bll.Implements.Security
{
    public class UserBll(IUserRepository repository) : IUserBll
    {
        private readonly IUserRepository _repository = repository;

        public async Task<ApiResponse> Create(User entity) => await _repository.Create(entity);

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll() => await _repository.GetAll();

        public async Task<ApiResponse> GetById(int id) => await _repository.GetById(id);

        public async Task<ApiResponse> Update(User entity) => await _repository.Update(entity);
    }
}
