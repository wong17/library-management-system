using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;
using LibraryManagementSystem.WebAPI.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;

namespace LibraryManagementSystem.WebAPI.Controllers
{
    [Route("api/monograph_loans")]
    [ApiController]
    public class MonographLoanController(IMonographLoanBll monographLoanBll, IMapper mapper,
        IHubContext<MonographLoanNotificationHub, ILoanNotification> hubContext,
        IHubContext<MonographNotificationHub, IMonographNotification> monographHubContext) : ControllerBase
    {
        private readonly IMonographLoanBll _monographLoanBll = monographLoanBll;
        private readonly IMapper _mapper = mapper;
        private readonly IHubContext<MonographLoanNotificationHub, ILoanNotification> _hubContext = hubContext;
        private readonly IHubContext<MonographNotificationHub, IMonographNotification> _monographHubContext = monographHubContext;

        /// <summary>
        /// Inserta un préstamo de monografia
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] MonographLoanInsertDto value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Préstamo de monografia es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _monographLoanBll.Create(_mapper.Map<MonographLoan>(value));
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            // Notifica a clientes con rol admin o bibliotecario
            await _hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await _monographHubContext.Clients.All.SendMonographStillAvailableNotification(true);

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

            var response = await _monographLoanBll.Delete(id);
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            // Notifica a clientes con rol admin o bibliotecario
            await _hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await _monographHubContext.Clients.All.SendMonographStillAvailableNotification(true);

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
            var response = await _monographLoanBll.GetAll();
            if ((response.IsSuccess == 1 || response.IsSuccess == 3) && response.StatusCode == HttpStatusCode.InternalServerError)
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

            var response = await _monographLoanBll.GetById(id);
            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
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
        public async Task<IActionResult> UpdateBorrowedMonographLoan(UpdateBorrowedMonographLoanDto updateBorrowedMonographLoanDto)
        {
            if (updateBorrowedMonographLoanDto is null)
                return BadRequest(new ApiResponse() { Message = "Dto es null", StatusCode = HttpStatusCode.BadRequest });

            var response = await _monographLoanBll.UpdateBorrowedMonographLoan(updateBorrowedMonographLoanDto);
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            // Notifica a clientes con rol admin o bibliotecario
            await _hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await _monographHubContext.Clients.All.SendMonographStillAvailableNotification(true);

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
        public async Task<IActionResult> UpdateReturnedMonographLoan(UpdateReturnedMonographLoanDto updateReturnedMonographLoanDto)
        {
            if (updateReturnedMonographLoanDto is null)
                return BadRequest(new ApiResponse() { Message = "Dto es null", StatusCode = HttpStatusCode.BadRequest });

            var response = await _monographLoanBll.UpdateReturnedMonographLoan(updateReturnedMonographLoanDto);
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            // Notifica a clientes con rol admin o bibliotecario
            await _hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await _monographHubContext.Clients.All.SendMonographStillAvailableNotification(true);

            return Ok(response);
        }
    }
}
