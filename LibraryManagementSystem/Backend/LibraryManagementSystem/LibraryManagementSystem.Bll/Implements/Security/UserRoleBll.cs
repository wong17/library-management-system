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
        public async Task<ApiResponse> Create(UserRoleInsertDto entity) => await repository.Create(mapper.Map<UserRole>(entity));

        public async Task<ApiResponse> CreateMany(IEnumerable<UserRoleInsertDto> entities) =>
            await repository.CreateMany(mapper.Map<IEnumerable<UserRole>>(entities));

        public async Task<ApiResponse> Delete(int userId, int roleId) => await repository.Delete(userId, roleId);

        public async Task<ApiResponse> GetAll()
        {
            var response = await repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<UserRole> userRoles)
                return response;
            // Retornar Dtos
            response.Result = mapper.Map<IEnumerable<UserRoleDto>>(userRoles);

            return response;
        }

        public async Task<ApiResponse> GetById(int userId, int roleId)
        {
            var response = await repository.GetById(userId, roleId);
            // Comprobar si hay un elemento
            if (response.Result is not UserRoleDto userRole)
                return response;
            // Retornar Dto
            response.Result = mapper.Map<UserRoleDto>(userRole);

            return response;
        }
    }
}