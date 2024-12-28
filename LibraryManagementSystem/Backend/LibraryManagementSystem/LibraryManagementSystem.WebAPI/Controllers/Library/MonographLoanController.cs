using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.WebAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace LibraryManagementSystem.WebAPI.Controllers.Library
{
    [Route("api/monograph_loans")]
    [ApiController]
    public class MonographLoanController(IMonographLoanBll monographLoanBll, IHubContext<MonographLoanNotificationHub, ILoanNotification> hubContext,
        IHubContext<MonographNotificationHub, IMonographNotification> monographHubContext) : ControllerBase
    {
        /// <summary>
        /// Inserta un préstamo de monografia
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] MonographLoanInsertDto? value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Préstamo de monografia es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await monographLoanBll.Create(value);
            switch (response)
            {
                case { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest }:
                    return BadRequest(response);

                case { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound }:
                    return NotFound(response);

                case { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError }:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            // Notifica a clientes con rol admin o bibliotecario
            await hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await monographHubContext.Clients.All.SendMonographNotification(true);

            return Ok(response);
        }

        /// <summary>
        /// Elimina un préstamo de monografia por su Id
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

            var response = await monographLoanBll.Delete(id);
            switch (response)
            {
                case { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest }:
                    return BadRequest(response);

                case { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound }:
                    return NotFound(response);

                case { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError }:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            // Notifica a clientes con rol admin o bibliotecario
            await hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await monographHubContext.Clients.All.SendMonographNotification(true);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve todos los préstamos de monografias
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await monographLoanBll.GetAll();
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un préstamo de monografia por su Id
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

            var response = await monographLoanBll.GetById(id);
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
        /// Devuelve un préstamo de monografia por su estado
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet("get_by_state/{state}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookLoanByState(string? state)
        {
            if (string.IsNullOrEmpty(state))
                return BadRequest(new ApiResponse() { Message = "State no puede ser null o vacio.", StatusCode = HttpStatusCode.BadRequest });

            var response = await monographLoanBll.GetMonographLoanByState(state);
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un préstamo de monografia por su carnet
        /// </summary>
        /// <param name="carnet"></param>
        /// <returns></returns>
        [HttpGet("get_by_student_carnet/{carnet}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookLoanByStudentCarnet(string? carnet)
        {
            if (string.IsNullOrEmpty(carnet))
                return BadRequest(new ApiResponse() { Message = "Carnet no puede ser null o vacio.", StatusCode = HttpStatusCode.BadRequest });

            var response = await monographLoanBll.GetMonographLoanByStudentCarnet(carnet);
            if (response.IsSuccess is ApiResponseCode.ValidationError or ApiResponseCode.DatabaseError && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Para aprobar una solicitud de préstamo de monografía, estado de la solicitud: CREADA -> PRESTADA
        /// </summary>
        /// <param name="updateBorrowedMonographLoanDto"></param>
        /// <returns></returns>
        [HttpPut("update_borrowed_monograph_loan")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBorrowedMonographLoan(UpdateBorrowedMonographLoanDto? updateBorrowedMonographLoanDto)
        {
            if (updateBorrowedMonographLoanDto is null)
                return BadRequest(new ApiResponse() { Message = "Dto es null", StatusCode = HttpStatusCode.BadRequest });

            var response = await monographLoanBll.UpdateBorrowedMonographLoan(updateBorrowedMonographLoanDto);
            switch (response)
            {
                case { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest }:
                    return BadRequest(response);

                case { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound }:
                    return NotFound(response);

                case { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError }:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            // Notifica a clientes con rol admin o bibliotecario
            await hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await monographHubContext.Clients.All.SendMonographNotification(true);

            return Ok(response);
        }

        /// <summary>
        /// Para hacer la devolución de una monografía, estado de la solicitud: PRESTADA -> DEVUELTA
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("update_returned_monograph_loan")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReturnedMonographLoan(UpdateReturnedMonographLoanDto? updateReturnedMonographLoanDto)
        {
            if (updateReturnedMonographLoanDto is null)
                return BadRequest(new ApiResponse() { Message = "Dto es null", StatusCode = HttpStatusCode.BadRequest });

            var response = await monographLoanBll.UpdateReturnedMonographLoan(updateReturnedMonographLoanDto);
            switch (response)
            {
                case { IsSuccess: ApiResponseCode.ValidationError, StatusCode: HttpStatusCode.BadRequest }:
                    return BadRequest(response);

                case { IsSuccess: ApiResponseCode.ResourceNotFound, StatusCode: HttpStatusCode.NotFound }:
                    return NotFound(response);

                case { IsSuccess: ApiResponseCode.DatabaseError, StatusCode: HttpStatusCode.InternalServerError }:
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
            }

            // Notifica a clientes con rol admin o bibliotecario
            await hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await monographHubContext.Clients.All.SendMonographNotification(true);

            return Ok(response);
        }
    }
}