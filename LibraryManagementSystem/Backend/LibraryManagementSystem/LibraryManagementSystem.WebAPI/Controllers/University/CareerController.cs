using LibraryManagementSystem.Bll.Interfaces.University;
using LibraryManagementSystem.Common.Runtime;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LibraryManagementSystem.WebAPI.Controllers.University
{
    [Route("api/careers")]
    [ApiController]
    public class CareerController(ICareerBll carrerBll) : ControllerBase
    {
        /// <summary>
        /// Devuelve todas las carreras
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await carrerBll.GetAll();
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve una carrera por su Id
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

            var response = await carrerBll.GetById(id);
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