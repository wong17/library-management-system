using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.WebAPI.Hubs.Implementations;
using LibraryManagementSystem.WebAPI.Hubs.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace LibraryManagementSystem.WebAPI.Controllers.Library
{
    [Route("api/publishers")]
    [ApiController]
    public class PublisherController(IPublisherBll publisherBll, IHubContext<PublisherNotificationHub, IPublisherNotification> publisherHubContext) : ControllerBase
    {
        /// <summary>
        /// Inserta una Editorial
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] PublisherInsertDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Editorial es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await publisherBll.Create(value);
            switch (response)
            {
                case { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest }:
                    return BadRequest(response);

                case { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError }:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);

                default:
                    // Notifica a todos los clientes
                    await publisherHubContext.Clients.All.SendPublisherNotification(true);

                    return Ok(response);
            }
        }

        /// <summary>
        /// Inserta varias Editoriales
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPost("create_many")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMany([FromBody] IEnumerable<PublisherInsertDto>? list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de Editoriales es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a insertar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await publisherBll.CreateMany(list);
            switch (response)
            {
                case { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest }:
                    return BadRequest(response);

                case { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError }:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);

                default:
                    // Notifica a todos los clientes
                    await publisherHubContext.Clients.All.SendPublisherNotification(true);

                    return Ok(response);
            }
        }

        /// <summary>
        /// Elimina una Editorial por su Id
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

            var response = await publisherBll.Delete(id);
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
                    await publisherHubContext.Clients.All.SendPublisherNotification(true);

                    return Ok(response);
            }
        }

        /// <summary>
        /// Devuelve todas las Editoriales
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await publisherBll.GetAll();
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve una Editorial por su Id
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

            var response = await publisherBll.GetById(id);
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
        /// Actualiza una Editorial
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("update")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromBody] PublisherUpdateDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Editorial es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await publisherBll.Update(value);
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
                    await publisherHubContext.Clients.All.SendPublisherNotification(true);

                    return Ok(response);
            }
        }

        /// <summary>
        /// Actualiza varias Editoriales
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [HttpPut("update_many")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMany([FromBody] IEnumerable<PublisherUpdateDto>? list)
        {
            if (list is null)
                return BadRequest(new ApiResponse() { Message = "Lista de Editoriales es null.", StatusCode = HttpStatusCode.BadRequest });

            if (!list.Any())
                return BadRequest(new ApiResponse() { Message = "La lista no tiene elementos a actualizar.", StatusCode = HttpStatusCode.BadRequest });

            var response = await publisherBll.UpdateMany(list);
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
                    await publisherHubContext.Clients.All.SendPublisherNotification(true);

                    return Ok(response);
            }
        }
    }
}