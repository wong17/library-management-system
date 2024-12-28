using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibraryManagementSystem.WebAPI.Controllers.Library
{
    [Route("api/book_subcategories")]
    [ApiController]
    public class BookSubCategoryController(IBookSubCategoryBll bookSubCategoryBll) : ControllerBase
    {
        /// <summary>
        /// Inserta una Sub categoria para el libro
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookSubCategoryInsertDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Sub categoria del libro es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await bookSubCategoryBll.Create(value);
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
        /// Inserta varias Sub categorias para el libro
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("create_many")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMany([FromBody] IEnumerable<BookSubCategoryInsertDto>? list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de Sub categorías para el libro es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a insertar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await bookSubCategoryBll.CreateMany(list);
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
        /// Elimina una sub categoria para el libro por su Id
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="subCategoryId"></param>
        /// <returns></returns>
        [HttpDelete("delete/{bookId:int}/{subCategoryId:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int bookId, int subCategoryId)
        {
            if (bookId <= 0 || subCategoryId <= 0)
                return BadRequest(new ApiResponse() { Message = "Id(s) no pueden ser menor o igual a 0.", StatusCode = HttpStatusCode.BadRequest });

            var response = await bookSubCategoryBll.Delete(bookId, subCategoryId);
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
        /// Devuelve todas las sub categorias del libro
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await bookSubCategoryBll.GetAll();
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve una sub categoria del libro por su Id
        /// </summary>
        /// <param name="bookId"></param>
        /// <param name="subCategoryId"></param>
        /// <returns></returns>
        [HttpGet("get_by_id/{bookId:int}/{subCategoryId:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int bookId, int subCategoryId)
        {
            if (bookId <= 0 || subCategoryId <= 0)
                return BadRequest(new ApiResponse() { Message = "Id(s) no puede ser menor o igual a 0.", StatusCode = HttpStatusCode.BadRequest });

            var response = await bookSubCategoryBll.GetById(bookId, subCategoryId);
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
        /// Actualiza varias sub categorias del libro
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPut("update_many")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMany([FromBody] IEnumerable<BookSubCategoryInsertDto>? list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de Sub categorías es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a actualizar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await bookSubCategoryBll.UpdateMany(list);
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