using LibraryManagementSystem.Bll.Interfaces.Security;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Security;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibraryManagementSystem.WebAPI.Controllers.Security
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController(IRoleBll roleBll) : ControllerBase
    {
        /// <summary>
        /// Inserta un Rol
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] RoleInsertDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Usuario es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await roleBll.Create(value);
            return response switch
            {
                { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest } => BadRequest(
                    response),
                { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError } =>
                    StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => Ok(response)
            };
        }

        /// <summary>
        /// Elimina un Rol por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponse() { Message = "Id no puede ser menor o igual a 0.", StatusCode = HttpStatusCode.BadRequest });

            var response = await roleBll.Delete(id);
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
        /// Devuelve todos los Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await roleBll.GetAll();
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un Rol por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get_by_id/{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponse() { Message = "Id no puede ser menor o igual a 0.", StatusCode = HttpStatusCode.BadRequest });

            var response = await roleBll.GetById(id);
            return response switch
            {
                { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound } => NotFound(
                    response),
                { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError } =>
                    StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => Ok(response)
            };
        }

        /// <summary>
        /// Actualiza un Rol
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] RoleUpdateDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Rol es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await roleBll.Update(value);
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
    }
}