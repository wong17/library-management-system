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
    [Route("api/book_loans")]
    [ApiController]
    public class BookLoanController(IBookLoanBll bookLoanBll, IMapper mapper, 
        IHubContext<BookLoanNotificationHub, ILoanNotification> hubContext,
        IHubContext<BookNotificationHub, IBookNotification> bookHubContext) : ControllerBase
    {
        private readonly IBookLoanBll _bookLoanBll = bookLoanBll;
        private readonly IMapper _mapper = mapper;
        private readonly IHubContext<BookLoanNotificationHub, ILoanNotification> _hubContext = hubContext;
        private readonly IHubContext<BookNotificationHub, IBookNotification> _bookHubContext = bookHubContext;

        /// <summary>
        /// Inserta un préstamo de libro
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookLoanInsertDto value)
        {
            if (value is null)
                return BadRequest(new ApiResponse() { Message = "Préstamo de libro es null.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _bookLoanBll.Create(_mapper.Map<BookLoan>(value));
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            // Notifica a clientes con rol admin o bibliotecario
            await _hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await _bookHubContext.Clients.All.SendBookNotification(true);

            return Ok(response);
        }

        /// <summary>
        /// Elimina un préstamo de libro por su Id
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

            var response = await _bookLoanBll.Delete(id);
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            // Notifica a clientes con rol admin o bibliotecario
            await _hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await _bookHubContext.Clients.All.SendBookNotification(true);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve todos los préstamos de libros
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            var response = await _bookLoanBll.GetAll();
            if ((response.IsSuccess == 1 || response.IsSuccess == 3) && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un préstamo de libro por su Id
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

            var response = await _bookLoanBll.GetById(id);
            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un préstamo de libro por su estado
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

            var response = await _bookLoanBll.GetBookLoanByState(state);
            if ((response.IsSuccess == 1 || response.IsSuccess == 3) && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un préstamo de libro por su tipo de prestamo
        /// </summary>
        /// <param name="typeOfLoan"></param>
        /// <returns></returns>
        [HttpGet("get_by_type_of_loan/{typeOfLoan}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBookLoanByTypeOfLoan(string? typeOfLoan)
        {
            if (string.IsNullOrEmpty(typeOfLoan))
                return BadRequest(new ApiResponse() { Message = "Tipo de préstamo no puede ser null o vacio.", StatusCode = HttpStatusCode.BadRequest });

            var response = await _bookLoanBll.GetBookLoanByTypeOfLoan(typeOfLoan);
            if ((response.IsSuccess == 1 || response.IsSuccess == 3) && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Devuelve un préstamo de libro por su carnet
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

            var response = await _bookLoanBll.GetBookLoanByStudentCarnet(carnet);
            if ((response.IsSuccess == 1 || response.IsSuccess == 3) && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            return Ok(response);
        }

        /// <summary>
        /// Para aprobar una solicitud de préstamo de libro, estado de la solicitud: CREADA -> PRESTADO
        /// </summary>
        /// <param name="updateBorrowedBookLoanDto"></param>
        /// <returns></returns>
        [HttpPut("update_borrowed_book_loan")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBorrowedBookLoan(UpdateBorrowedBookLoanDto updateBorrowedBookLoanDto)
        {
            if (updateBorrowedBookLoanDto is null)
                return BadRequest(new ApiResponse() { Message = "Dto es null", StatusCode = HttpStatusCode.BadRequest });

            var response = await _bookLoanBll.UpdateBorrowedBookLoan(updateBorrowedBookLoanDto);
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            // Notifica a clientes con rol admin o bibliotecario
            await _hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await _bookHubContext.Clients.All.SendBookNotification(true);

            return Ok(response);
        }

        /// <summary>
        /// Para hacer la devolución de un libro, estado de la solicitud: PRESTADO -> DEVUELTO
        /// </summary>
        /// <param name="updateReturnedBookLoanDto"></param>
        /// <returns></returns>
        [HttpPut("update_returned_book_loan")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateReturnedBookLoan(UpdateReturnedBookLoanDto updateReturnedBookLoanDto)
        {
            if (updateReturnedBookLoanDto is null)
                return BadRequest(new ApiResponse() { Message = "Dto es null", StatusCode = HttpStatusCode.BadRequest });

            var response = await _bookLoanBll.UpdateReturnedBookLoan(updateReturnedBookLoanDto);
            if (response.IsSuccess == 1 && response.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(response);

            if (response.IsSuccess == 2 && response.StatusCode == HttpStatusCode.NotFound)
                return NotFound(response);

            if (response.IsSuccess == 3 && response.StatusCode == HttpStatusCode.InternalServerError)
                return StatusCode(StatusCodes.Status500InternalServerError, response);

            // Notifica a clientes con rol admin o bibliotecario
            await _hubContext.Clients.All.SendLoanNotification(true);
            // Notifica a todos los clientes
            await _bookHubContext.Clients.All.SendBookNotification(true);

            return Ok(response);
        }
    }
}
