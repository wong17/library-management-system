using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibraryManagementSystem.WebAPI.Controllers.Library
{
    [Route("api/monograph_authors")]
    [ApiController]
    public class MonographAuthorController(IMonographAuthorBll monographAuthorBll) : ControllerBase
    {
        /// <summary>
        /// Inserta un autor para la monografia
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] MonographAuthorInsertDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Autor es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await monographAuthorBll.Create(value);
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
        /// Inserta varios autores para la monografia
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("create_many")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMany([FromBody] IEnumerable<MonographAuthorInsertDto>? list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de autores para la monografia es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a insertar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await monographAuthorBll.CreateMany(list);
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
        /// Elimina un autor para la monografia por su Id
        /// </summary>
        /// <param name="monographId"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [HttpDelete("delete/{monographId:int}/{authorId:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int monographId, int authorId)
        {
            if (monographId <= 0 || authorId <= 0)
                return BadRequest(new ApiResponse() { Message = "Id(s) no pueden ser menor o igual a 0.", StatusCode = HttpStatusCode.BadRequest });

            var response = await monographAuthorBll.Delete(monographId, authorId);
            return response.IsSuccess switch
            {
                ApiResponseCode.ValidationError when response.StatusCode == HttpStatusCode.BadRequest => BadRequest(
                    response),
                ApiResponseCode.ResourceNotFound when response.StatusCode == HttpStatusCode.NotFound => NotFound(
                    response),
                ApiResponseCode.DatabaseError when response.StatusCode == HttpStatusCode.InternalServerError =>
                    StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => Ok(response)
            };
        }

        /// <summary>
        /// Devuelve todos los autores de las monografias
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await monographAuthorBll.GetAll();
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un autor de la monografia por su Id
        /// </summary>
        /// <param name="monographId"></param>
        /// <param name="authorId"></param>
        /// <returns></returns>
        [HttpGet("get_by_id/{monographId:int}/{authorId:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int monographId, int authorId)
        {
            if (monographId <= 0 || authorId <= 0)
                return BadRequest(new ApiResponse() { Message = "Id(s) no puede ser menor o igual a 0.", StatusCode = HttpStatusCode.BadRequest });

            var response = await monographAuthorBll.GetById(monographId, authorId);
            return response switch
            {
                { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.NotFound } => NotFound(
                    response),
                { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError } =>
                    StatusCode(StatusCodes.Status500InternalServerError, response),
                _ => Ok(response)
            };
        }

        /// <summary>
        /// Actualiza varios autores de la monografía
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPut("update_many")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMany([FromBody] IEnumerable<MonographAuthorInsertDto>? list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de Autores es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a actualizar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await monographAuthorBll.UpdateMany(list);
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