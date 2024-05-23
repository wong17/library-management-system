using LibraryManagementSystem.Bll.Interfaces.Security;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Security;
using LibraryManagementSystem.Entities.Models.Security;

namespace LibraryManagementSystem.Bll.Implements.Security
{
    public class UserRoleBll(IUserRoleRepository repository) : IUserRoleBll
    {
        private readonly IUserRoleRepository _repository = repository;

        public async Task<ApiResponse> Create(UserRole entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<UserRole> entities) => await _repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int userId, int roleId) => await _repository.Delete(userId, roleId);

        public async Task<ApiResponse> GetAll() => await _repository.GetAll();

        public async Task<ApiResponse> GetById(int userId, int roleId) => await _repository.GetById(userId, roleId);
    }
}
