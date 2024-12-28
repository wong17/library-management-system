using LibraryManagementSystem.Bll.Interfaces.Security;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Security;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibraryManagementSystem.WebAPI.Controllers.Security
{
    [Route("api/user_roles")]
    [ApiController]
    public class UserRoleController(IUserRoleBll userRoleBll) : ControllerBase
    {
        /// <summary>
        /// Inserta un rol para el usuario
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] UserRoleInsertDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Rol es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await userRoleBll.Create(value);
            return response switch
            {
                { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest } => BadRequest(
                    response),
                { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound } => NotFound(
                    response),
                { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError } =>
                    StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => Ok(response)
            };
        }

        /// <summary>
        /// Inserta varios roles para el usuario
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("create_many")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMany([FromBody] IEnumerable<UserRoleInsertDto>? list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de roles para el usuario es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a insertar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await userRoleBll.CreateMany(list);
            return response switch
            {
                { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest } => BadRequest(
                    response),
                { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound } => NotFound(
                    response),
                { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError } =>
                    StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => Ok(response)
            };
        }

        /// <summary>
        /// Elimina un rol del usuario por su id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpDelete("delete/{userId:int}/{roleId:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int userId, int roleId)
        {
            if (userId <= 0 || roleId <= 0)
                return BadRequest(new ApiResponse() { Message = "Id(s) no pueden ser menor o igual a 0.", StatusCode = HttpStatusCode.BadRequest });

            var response = await userRoleBll.Delete(userId, roleId);
            return response switch
            {
                { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest } => BadRequest(
                    response),
                { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound } => NotFound(
                    response),
                { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError } =>
                    StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => Ok(response)
            };
        }

        /// <summary>
        /// Devuelve todos los roles del usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await userRoleBll.GetAll();
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un rol del usuario por su Id
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet("get_by_id/{userId:int}/{roleId:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int userId, int roleId)
        {
            if (userId <= 0 || roleId <= 0)
                return BadRequest(new ApiResponse() { Message = "Id(s) no puede ser menor o igual a 0.", StatusCode = HttpStatusCode.BadRequest });

            var response = await userRoleBll.GetById(userId, roleId);
            return response switch
            {
                { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound } => NotFound(
                    response),
                { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError } =>
                    StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => Ok(response)
            };
        }
    }
}