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
    public class SubCategoryController(ISubCategoryBll subCategoryBll, IMapper mapper) : ControllerBase
    {
        private readonly ISubCategoryBll _subCategoryBll = subCategoryBll;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Inserta una SubCategoria
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] SubCategoryInsertDto value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "SubCategoría es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _subCategoryBll.Create(_mapper.Map<SubCategory>(value));
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Inserta varias SubCategorías
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("CreateMany")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMany([FromBody] IEnumerable<SubCategoryInsertDto> list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de SubCategorías es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a insertar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _subCategoryBll.CreateMany(_mapper.Map<IEnumerable<SubCategory>>(list));
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Elimina una SubCategoría por su Id
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

            var response = await _subCategoryBll.Delete(id);
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve todas las SubCategorías
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _subCategoryBll.GetAll();
            if ((response.IsSuccess == 1 || response.IsSuccess == 3) && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve una SubCategoría por su Id
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

            var response = await _subCategoryBll.GetById(id);
            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza una SubCategoría
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] SubCategoryUpdateDto value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Categoria es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _subCategoryBll.Update(_mapper.Map<SubCategory>(value));
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Actualiza varias SubCategorías
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPut("UpdateMany")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMany([FromBody] IEnumerable<SubCategoryUpdateDto> list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de SubCategorias es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a actualizar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _subCategoryBll.UpdateMany(_mapper.Map<IEnumerable<SubCategory>>(list));
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
