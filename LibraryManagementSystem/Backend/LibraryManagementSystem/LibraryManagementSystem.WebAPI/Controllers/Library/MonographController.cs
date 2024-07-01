using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace LibraryManagementSystem.WebAPI.Controllers
{
    [Route("api/monographs")]
    [ApiController]
    public class MonographController(IMonographBll monographBll, IMapper mapper) : ControllerBase
    {
        private readonly IMonographBll _monographBll = monographBll;
        private readonly IMapper _mapper = mapper;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Inserta una monografia
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] MonographInsertDto value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Libro es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _monographBll.Create(_mapper.Map<Monograph>(value));
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Elimina una monografia por su Id
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

            var response = await _monographBll.Delete(id);
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve todas las monografias
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _monographBll.GetAll();
            if ((response.IsSuccess == 1 || response.IsSuccess == 3) && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve una monografia por su Id
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

            var response = await _monographBll.GetById(id);
            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelva las monografias filtradas por autores, carreras y fecha de presentación
        /// </summary>
        /// <param name="filterParamsDto"></param>
        /// <returns></returns>
        [HttpGet("get_filtered_monograph")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFilteredMonograph([FromQuery] string filterParamsDto)
        {
            if (filterParamsDto is null)
                return BadRequest(new ApiResponse() { Message = "FilterBookDto json es null.", StatusCode = HttpStatusCode.BadRequest });

            var filterMonographDto = JsonSerializer.Deserialize<FilterMonographDto>(filterParamsDto, _jsonOptions);
            if (filterMonographDto is null)
                return BadRequest(new ApiResponse() { Message = "FilterMonographDto es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _monographBll.GetFilteredMonograph(filterMonographDto);
            if ((response.IsSuccess == 1 || response.IsSuccess == 3) && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza una monografia
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] MonographUpdateDto value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Monografia es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _monographBll.Update(_mapper.Map<Monograph>(value));
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
