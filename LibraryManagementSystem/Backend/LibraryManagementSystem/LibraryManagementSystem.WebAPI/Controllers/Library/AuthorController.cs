using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibraryManagementSystem.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController(IAuthorBll authorBll, IMapper mapper) : ControllerBase
    {
        private readonly IAuthorBll _authorBll = authorBll;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Inserta un Autor
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorInsertDto value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Autor es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _authorBll.Create(_mapper.Map<Author>(value));
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Inserta varios Autores
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("CreateMany")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMany([FromBody] IEnumerable<AuthorInsertDto> list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de Autores es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a insertar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _authorBll.CreateMany(_mapper.Map<IEnumerable<Author>>(list));
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Elimina un Autor por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponse() { Message = "Id no puede ser menor o igual a 0.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _authorBll.Delete(id);
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve todos los Autores
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _authorBll.GetAll();
            if ((response.IsSuccess == 1 || response.IsSuccess == 3) && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un Autor por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest(new ApiResponse() { Message = "Id no puede ser menor o igual a 0.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _authorBll.GetById(id);
            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza un Autor
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] AuthorUpdateDto value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Autor es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _authorBll.Update(_mapper.Map<Author>(value));
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza varios Autores
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPut("UpdateMany")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMany([FromBody] IEnumerable<AuthorUpdateDto> list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de Autores es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a actualizar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _authorBll.UpdateMany(_mapper.Map<IEnumerable<Author>>(list));
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }
    }
}
