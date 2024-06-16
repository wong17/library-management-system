using LibraryManagementSystem.Bll.Interfaces.University;
using LibraryManagementSystem.Common.Runtime;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibraryManagementSystem.WebAPI.Controllers.University
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController(IStudentBll studentBll) : ControllerBase
    {
        private readonly IStudentBll _studentBll = studentBll;

        /// <summary>
        /// Devuelve todos los estudiantes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _studentBll.GetAll();
            if ((response.IsSuccess == 1 || response.IsSuccess == 3) && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un estudiante por su Id
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

            var response = await _studentBll.GetById(id);
            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }
    }
}
