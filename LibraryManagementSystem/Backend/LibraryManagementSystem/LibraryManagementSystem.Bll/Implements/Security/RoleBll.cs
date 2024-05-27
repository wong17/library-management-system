using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Security;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Security;
using LibraryManagementSystem.Entities.Dtos.Security;
using LibraryManagementSystem.Entities.Models.Security;

namespace LibraryManagementSystem.Bll.Implements.Security
{
    public class RoleBll(IRoleRepository repository, IMapper mapper) : IRoleBll
    {
        private readonly IRoleRepository _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(Role entity) => await _repository.Create(entity);

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await _repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<Role> roles)
                return response;
            // Retornar Dtos
            response.Result = _mapper.Map<IEnumerable<RoleDto>>(roles);

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await _repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not Role role)
                return response;
            // Retornar Dto
            response.Result = _mapper.Map<RoleDto>(role);

            return response;
        }

        public async Task<ApiResponse> Update(Role entity) => await _repository.Update(entity);
    }
}
