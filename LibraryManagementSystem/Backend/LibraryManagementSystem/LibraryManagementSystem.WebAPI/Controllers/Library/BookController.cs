using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.WebAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Text.Json;

namespace LibraryManagementSystem.WebAPI.Controllers.Library
{
    [Route("api/books")]
    [ApiController]
    public class BookController(IBookBll bookBll, IHubContext<BookNotificationHub, IBookNotification> bookHubContext) : ControllerBase
    {
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Inserta un Libro
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookInsertDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Libro es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await bookBll.Create(value);
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
                    await bookHubContext.Clients.All.SendBookNotification(true);

                    return Ok(response);
            }
        }

        /// <summary>
        /// Elimina un Libro por su Id
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

            var response = await bookBll.Delete(id);
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
                    await bookHubContext.Clients.All.SendBookNotification(true);

                    return Ok(response);
            }
        }

        /// <summary>
        /// Devuelve todos los Libros
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await bookBll.GetAll();
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un Libro por su Id
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

            var response = await bookBll.GetById(id);
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
        /// Devuelva los libros filtrados por autores, editoriales, categorías, sub categorías y año de publicación
        /// </summary>
        /// <param name="filterParamsDto"></param>
        /// <returns></returns>
        [HttpGet("get_filtered_book")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFilteredBook([FromQuery] string? filterParamsDto)
        {
            if (filterParamsDto is null)
                return BadRequest(new ApiResponse() { Message = "FilterBookDto json es null.", StatusCode = HttpStatusCode.BadRequest });

            FilterBookDto? filterBookDto;
            try
            {
                filterBookDto = JsonSerializer.Deserialize<FilterBookDto>(filterParamsDto, _jsonOptions);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse() { Message = ex.Message, StatusCode = HttpStatusCode.BadRequest });
            }

            if (filterBookDto is null)
                return BadRequest(new ApiResponse() { Message = "FilterBookDto es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await bookBll.GetFilteredBook(filterBookDto);
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza un Libro
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] BookUpdateDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Libro es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await bookBll.Update(value);
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
                    await bookHubContext.Clients.All.SendBookNotification(true);

                    return Ok(response);
            }
        }
    }
}