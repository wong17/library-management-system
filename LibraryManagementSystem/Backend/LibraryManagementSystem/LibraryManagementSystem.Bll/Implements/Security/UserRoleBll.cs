using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Security;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Security;
using LibraryManagementSystem.Entities.Dtos.Security;
using LibraryManagementSystem.Entities.Models.Security;

namespace LibraryManagementSystem.Bll.Implements.Security
{
    public class UserRoleBll(IUserRoleRepository repository, IMapper mapper) : IUserRoleBll
    {
        private readonly IUserRoleRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(UserRole entity) => await _repository.Create(entity);

        public async Task<ApiResponse> CreateMany(IEnumerable<UserRole> entities) => await _repository.CreateMany(entities);

        public async Task<ApiResponse> Delete(int userId, int roleId) => await _repository.Delete(userId, roleId);

        public async Task<ApiResponse> GetAll()
        {
            var response = await _repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<UserRole> userRoles)
                return response;
            // Retornar Dtos
            response.Result = _mapper.Map<IEnumerable<UserRoleDto>>(userRoles);

            return response;
        }

        public async Task<ApiResponse> GetById(int userId, int roleId)
        {
            var response = await _repository.GetById(userId, roleId);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not UserRoleDto userRole)
                return response;
            // Retornar Dto
            response.Result = _mapper.Map<UserRoleDto>(userRole);

            return response;
        }
    }
}
