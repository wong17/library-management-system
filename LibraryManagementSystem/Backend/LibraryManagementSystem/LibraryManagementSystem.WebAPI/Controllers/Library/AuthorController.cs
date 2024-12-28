using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.WebAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace LibraryManagementSystem.WebAPI.Controllers.Library
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController(IAuthorBll authorBll, IHubContext<AuthorNotificationHub, IAuthorNotification> authorHubContext) : ControllerBase
    {
        /// <summary>
        /// Inserta un Autor
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorInsertDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Autor es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await authorBll.Create(value);
            switch (response)
            {
                case { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest }:
                    return BadRequest(response);

                case { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError }:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);

                default:
                    // Notifica a todos los clientes
                    await authorHubContext.Clients.All.SendAuthorNotification(true);

                    return Ok(response);
            }
        }

        /// <summary>
        /// Inserta varios Autores
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("create_many")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMany([FromBody] IEnumerable<AuthorInsertDto>? list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de Autores es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a insertar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await authorBll.CreateMany(list);
            switch (response)
            {
                case { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest }:
                    return BadRequest(response);

                case { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError }:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);

                default:
                    // Notifica a todos los clientes
                    await authorHubContext.Clients.All.SendAuthorNotification(true);

                    return Ok(response);
            }
        }

        /// <summary>
        /// Elimina un Autor por su Id
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

            var response = await authorBll.Delete(id);
            switch (response)
            {
                case { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest }:
                    return BadRequest(response);

                case { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound }:
                    return NotFound(response);
            }

            if (response is { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError })
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            // Notifica a todos los clientes
            await authorHubContext.Clients.All.SendAuthorNotification(true);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve todos los Autores
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await authorBll.GetAll();
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un Autor por su Id
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

            var response = await authorBll.GetById(id);
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
        /// Actualiza un Autor
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] AuthorUpdateDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Autor es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await authorBll.Update(value);
            switch (response)
            {
                case { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest }:
                    return BadRequest(response);

                case { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound }:
                    return NotFound(response);

                case { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError }:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);

                default:
                    // Notifica a todos los clientes
                    await authorHubContext.Clients.All.SendAuthorNotification(true);

                    return Ok(response);
            }
        }

        /// <summary>
        /// Actualiza varios Autores
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPut("update_many")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMany([FromBody] IEnumerable<AuthorUpdateDto>? list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de Autores es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a actualizar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await authorBll.UpdateMany(list);
            switch (response)
            {
                case { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest }:
                    return BadRequest(response);

                case { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound }:
                    return NotFound(response);

                case { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError }:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);

                default:
                    // Notifica a todos los clientes
                    await authorHubContext.Clients.All.SendAuthorNotification(true);

                    return Ok(response);
            }
        }
    }
}