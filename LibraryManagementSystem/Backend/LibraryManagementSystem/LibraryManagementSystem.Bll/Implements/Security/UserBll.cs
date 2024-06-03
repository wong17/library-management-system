using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Security;
using LibraryManagementSystem.Common.Helpers;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Security;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Dtos.Security;
using LibraryManagementSystem.Entities.Models.Library;
using LibraryManagementSystem.Entities.Models.Security;

namespace LibraryManagementSystem.Bll.Implements.Security
{
    public class UserBll(IUserRepository repository, IRoleBll roleBll, IUserRoleBll userRoleBll, IMapper mapper) : IUserBll
    {
        private readonly IUserRepository _repository = repository;
        private readonly IRoleBll _roleBll = roleBll;
        private readonly IUserRoleBll _userRoleBll = userRoleBll;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(User entity) => await _repository.Create(entity);

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await _repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<User> users)
                return response;

            // Convertir users a UserDto
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users).ToList();

            // Comprobar si hay roles
            var rolesResponse = _roleBll.GetAll();
            var userRolesResponse = _userRoleBll.GetAll();
            if ((rolesResponse.Result.Result is not null && rolesResponse.Result.Result is IEnumerable<RoleDto> roles) && 
                (userRolesResponse.Result.Result is not null && userRolesResponse.Result.Result is IEnumerable<UserRoleDto> userRoles))
            {
                var rolesDictionary = roles.ToDictionary(r => r.RoleId);
                var userRolesDictionary = ListHelper.ListToDictionary(userRoles.ToList(), ur => ur.UserId, ur => ur.RoleId);

                for (int i = 0; i < usersDto.Count; i++)
                {
                    var userDto = usersDto[i];
                    var allRoles = new List<RoleDto>();

                    // Si no se puede obtener la lista con id de todos los roles del usuario actual...
                    if (!userRolesDictionary.TryGetValue(userDto.UserId, out var rolesList))
                        continue;

                    // Recorrer lista de id de roles para obtener el RoleDto
                    for (int j = 0; j < rolesList.Count; j++)
                    {
                        var authorId = rolesList[j];
                        // Obtener role en base a su id
                        if (rolesDictionary.TryGetValue(authorId, out var authorDto))
                        {
                            allRoles.Add(authorDto);
                        }
                    }
                    // Asignar lista
                    userDto.Roles = allRoles;
                }
            }

            // Retornar Dtos
            response.Result = usersDto;

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await _repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not User user)
                return response;

            // Convertir user a UserDto
            var userDto = _mapper.Map<UserDto>(user);

            // Comprobar si hay roles
            var rolesResponse = _roleBll.GetAll();
            var userRolesResponse = _userRoleBll.GetAll();
            if ((rolesResponse.Result.Result is not null && rolesResponse.Result.Result is IEnumerable<RoleDto> roles) &&
                (userRolesResponse.Result.Result is not null && userRolesResponse.Result.Result is IEnumerable<UserRoleDto> userRoles))
            {
                var rolesDictionary = roles.ToDictionary(r => r.RoleId);
                var userRolesDictionary = ListHelper.ListToDictionary(userRoles.ToList(), ur => ur.UserId, ur => ur.RoleId);

                var allRoles = new List<RoleDto>();

                // Si no se puede obtener la lista con id de todos los roles del usuario actual...
                if (userRolesDictionary.TryGetValue(userDto.UserId, out var rolesList))
                {
                    // Recorrer lista de id de roles para obtener el RoleDto
                    for (int j = 0; j < rolesList.Count; j++)
                    {
                        var authorId = rolesList[j];
                        // Obtener role en base a su id
                        if (rolesDictionary.TryGetValue(authorId, out var authorDto))
                        {
                            allRoles.Add(authorDto);
                        }
                    }
                    // Asignar lista
                    userDto.Roles = allRoles;
                }
            }

            // Retornar Dto
            response.Result = userDto;

            return response;
        }

        public async Task<ApiResponse> Update(User entity) => await _repository.Update(entity);

        public async Task<ApiResponse> LogIn(User entity)
        {
            var response = await _repository.LogIn(entity);

            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not User user)
                return response;

            // Convertir user a UserDto
            var userDto = _mapper.Map<UserDto>(user);

            // Comprobar si hay roles
            var rolesResponse = _roleBll.GetAll();
            var userRolesResponse = _userRoleBll.GetAll();
            if ((rolesResponse.Result.Result is not null && rolesResponse.Result.Result is IEnumerable<RoleDto> roles) &&
                (userRolesResponse.Result.Result is not null && userRolesResponse.Result.Result is IEnumerable<UserRoleDto> userRoles))
            {
                var rolesDictionary = roles.ToDictionary(r => r.RoleId);
                var userRolesDictionary = ListHelper.ListToDictionary(userRoles.ToList(), ur => ur.UserId, ur => ur.RoleId);

                var allRoles = new List<RoleDto>();

                // Si no se puede obtener la lista con id de todos los roles del usuario actual...
                if (userRolesDictionary.TryGetValue(userDto.UserId, out var rolesList))
                {
                    // Recorrer lista de id de roles para obtener el RoleDto
                    for (int j = 0; j < rolesList.Count; j++)
                    {
                        var authorId = rolesList[j];
                        // Obtener role en base a su id
                        if (rolesDictionary.TryGetValue(authorId, out var authorDto))
                        {
                            allRoles.Add(authorDto);
                        }
                    }
                    // Asignar lista
                    userDto.Roles = allRoles;
                }
            }

            // Retornar Dto
            response.Result = userDto;

            return response;
        }
    }
}
