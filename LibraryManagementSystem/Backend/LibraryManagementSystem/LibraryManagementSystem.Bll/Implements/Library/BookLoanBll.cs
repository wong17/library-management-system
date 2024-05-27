using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Bll.Interfaces.University;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Dtos.University;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class BookLoanBll(IBookLoanRepository repository, IBookBll bookBll, IStudentBll studentBll, IMapper mapper) : IBookLoanBll
    {
        private readonly IBookLoanRepository _repository = repository;
        private readonly IBookBll _bookBll = bookBll;
        private readonly IStudentBll _studentBll = studentBll;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(BookLoan entity) => await _repository.Create(entity);

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await _repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<BookLoan> bookLoans)
                return response;

            // Obtener ids de los estudiantes de todos los prestamos
            var studentIds = bookLoans.Select(s => s.StudentId).ToList();
            // Obtener ids de los libros de todos los prestamos
            var bookIds = bookLoans.Select(b => b.BookId).ToList();
            // Convertir bookloans a BookLoanDto
            var bookLoansDto = _mapper.Map<IEnumerable<BookLoanDto>>(bookLoans).ToList();

            // 1. Comprobar si hay estudiantes
            var studentsResponse = await _studentBll.GetAll();
            if (studentsResponse.Result is not null && studentsResponse.Result is IEnumerable<StudentDto> students)
            {
                var studentsDictionary = students.ToDictionary(s => s.StudentId);

                for(int i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];
                    var studentId = studentIds[i];

                    if(studentsDictionary.TryGetValue(studentId, out var studentDto))
                    {
                        bookLoanDto.Student = studentDto;
                    }
                }
            }

            // 2. Comprobar si hay libros
            var booksResponse = await _bookBll.GetAll();
            if (booksResponse.Result is not null && booksResponse.Result is IEnumerable<BookDto> books)
            {
                var booksDictionary = books.ToDictionary(b => b.BookId);

                for (int i = 0;i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];
                    var bookId = bookIds[i];

                    if (booksDictionary.TryGetValue(bookId, out var bookDto))
                    {
                        bookLoanDto.Book = bookDto;
                    }
                }
            }

            // Retornar Dtos
            response.Result = bookLoansDto;

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await _repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not BookLoan bookLoan)
                return response;

            // Obtener id del estudiante que realizo el prestamo
            var studentId = bookLoan.StudentId;
            // Obtener id del libro que se presto
            var bookId = bookLoan.BookId;
            // Convertir bookloans a BookLoanDto
            var bookLoanDto = _mapper.Map<BookLoanDto>(bookLoan);

            // 1. Comprobar si existe el estudiante (deberia...)
            var studentResponse = await _studentBll.GetById(studentId);
            if (studentResponse.Result is not null && studentResponse.Result is StudentDto student)
            {
                bookLoanDto.Student = student;
            }

            // 2. Comprobar si existe el libro (deberia...)
            var bookResponse = await _bookBll.GetById(bookId);
            if (bookResponse.Result is not null && bookResponse.Result is BookDto book)
            {
                bookLoanDto.Book = book;
            }

            response.Result = bookLoanDto;

            return response;
        }

        public async Task<ApiResponse> UpdateBorrowedBookLoan(int bookLoanId, DateTime dueDate) => await _repository.UpdateBorrowedBookLoan(bookLoanId, dueDate);

        public async Task<ApiResponse> UpdateReturnedBookLoan(int bookLoanId) => await _repository.UpdateReturnedBookLoan(bookLoanId);
    }
}
