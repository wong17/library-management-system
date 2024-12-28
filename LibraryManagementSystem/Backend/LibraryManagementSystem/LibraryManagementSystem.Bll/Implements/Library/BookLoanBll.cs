using AutoMapper;
using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Bll.Interfaces.Security;
using LibraryManagementSystem.Bll.Interfaces.University;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Dtos.Security;
using LibraryManagementSystem.Entities.Dtos.University;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class BookLoanBll(IBookLoanRepository repository, IBookBll bookBll, IStudentBll studentBll, IUserBll userBll, IMapper mapper) : IBookLoanBll
    {
        public async Task<ApiResponse> Create(BookLoanInsertDto entity) => await repository.Create(mapper.Map<BookLoan>(entity));

        public async Task<ApiResponse> Delete(int id) => await repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<BookLoan> bookLoans)
                return response;

            // Obtener ids de los estudiantes de todos los prestamos
            var bookLoanList = bookLoans.ToList();
            var studentIds = bookLoanList.Select(b => b.StudentId).ToList();
            // Obtener ids de los libros de todos los prestamos
            var bookIds = bookLoanList.Select(b => b.BookId).ToList();
            // Obtener ids de los usuarios que aprobaron el prestamo del libro
            var borrowedUserIds = bookLoanList.Select(b => b.BorrowedUserId).ToList();
            // Obtener ids de los usuarios que recibieron el libro
            var returnedUserIds = bookLoanList.Select(b => b.ReturnedUserId).ToList();
            // Convertir bookloans a BookLoanDto
            var bookLoansDto = mapper.Map<IEnumerable<BookLoanDto>>(bookLoans).ToList();

            // 1. Comprobar si hay estudiantes
            var studentsResponse = await studentBll.GetAll();
            if (studentsResponse.Result is IEnumerable<StudentDto> students)
            {
                var studentsDictionary = students.ToDictionary(s => s.StudentId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];
                    var studentId = studentIds[i];

                    if (studentsDictionary.TryGetValue(studentId, out var studentDto))
                    {
                        bookLoanDto.Student = studentDto;
                    }
                }
            }

            // 2. Comprobar si hay libros
            var booksResponse = await bookBll.GetAll();
            if (booksResponse.Result is IEnumerable<BookDto> books)
            {
                var booksDictionary = books.ToDictionary(b => b.BookId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];
                    var bookId = bookIds[i];

                    if (booksDictionary.TryGetValue(bookId, out var bookDto))
                    {
                        bookLoanDto.Book = bookDto;
                    }
                }
            }

            // 3. Borrowed users y Returned users
            var usersResponse = await userBll.GetAll();
            if (usersResponse.Result is IEnumerable<UserDto> users)
            {
                var usersDictionary = users.ToDictionary(u => u.UserId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];

                    var borrowedUserId = borrowedUserIds[i];
                    var returnedUserId = returnedUserIds[i];

                    if (usersDictionary.TryGetValue(borrowedUserId, out var borrowedUserDto))
                    {
                        bookLoanDto.BorrowedUser = borrowedUserDto;
                    }
                    if (usersDictionary.TryGetValue(returnedUserId, out var returnedUserDto))
                    {
                        bookLoanDto.ReturnedUser = returnedUserDto;
                    }
                }
            }

            // Retornar Dtos
            response.Result = bookLoansDto;

            return response;
        }

        public async Task<ApiResponse> GetBookLoanByState(string? state)
        {
            var response = await repository.GetBookLoanByState(state);
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<BookLoan> bookLoans)
                return response;

            // Obtener ids de los estudiantes de todos los prestamos
            var bookLoanList = bookLoans.ToList();
            var studentIds = bookLoanList.Select(b => b.StudentId).ToList();
            // Obtener ids de los libros de todos los prestamos
            var bookIds = bookLoanList.Select(b => b.BookId).ToList();
            // Obtener ids de los usuarios que aprobaron el prestamo del libro
            var borrowedUserIds = bookLoanList.Select(b => b.BorrowedUserId).ToList();
            // Obtener ids de los usuarios que recibieron el libro
            var returnedUserIds = bookLoanList.Select(b => b.ReturnedUserId).ToList();
            // Convertir bookloans a BookLoanDto
            var bookLoansDto = mapper.Map<IEnumerable<BookLoanDto>>(bookLoans).ToList();

            // 1. Comprobar si hay estudiantes
            var studentsResponse = await studentBll.GetAll();
            if (studentsResponse.Result is IEnumerable<StudentDto> students)
            {
                var studentsDictionary = students.ToDictionary(s => s.StudentId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];
                    var studentId = studentIds[i];

                    if (studentsDictionary.TryGetValue(studentId, out var studentDto))
                    {
                        bookLoanDto.Student = studentDto;
                    }
                }
            }

            // 2. Comprobar si hay libros
            var booksResponse = await bookBll.GetAll();
            if (booksResponse.Result is IEnumerable<BookDto> books)
            {
                var booksDictionary = books.ToDictionary(b => b.BookId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];
                    var bookId = bookIds[i];

                    if (booksDictionary.TryGetValue(bookId, out var bookDto))
                    {
                        bookLoanDto.Book = bookDto;
                    }
                }
            }

            // 3. Borrowed users y Returned users
            var usersResponse = await userBll.GetAll();
            if (usersResponse.Result is IEnumerable<UserDto> users)
            {
                var usersDictionary = users.ToDictionary(u => u.UserId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];

                    var borrowedUserId = borrowedUserIds[i];
                    var returnedUserId = returnedUserIds[i];

                    if (usersDictionary.TryGetValue(borrowedUserId, out var borrowedUserDto))
                    {
                        bookLoanDto.BorrowedUser = borrowedUserDto;
                    }
                    if (usersDictionary.TryGetValue(returnedUserId, out var returnedUserDto))
                    {
                        bookLoanDto.ReturnedUser = returnedUserDto;
                    }
                }
            }

            // Retornar Dtos
            response.Result = bookLoansDto;

            return response;
        }

        public async Task<ApiResponse> GetBookLoanByStudentCarnet(string? carnet)
        {
            var response = await repository.GetBookLoanByStudentCarnet(carnet);
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<BookLoan> bookLoans)
                return response;

            // Obtener ids de los estudiantes de todos los prestamos
            var bookLoanList = bookLoans.ToList();
            var studentIds = bookLoanList.Select(b => b.StudentId).ToList();
            // Obtener ids de los libros de todos los prestamos
            var bookIds = bookLoanList.Select(b => b.BookId).ToList();
            // Obtener ids de los usuarios que aprobaron el prestamo del libro
            var borrowedUserIds = bookLoanList.Select(b => b.BorrowedUserId).ToList();
            // Obtener ids de los usuarios que recibieron el libro
            var returnedUserIds = bookLoanList.Select(b => b.ReturnedUserId).ToList();
            // Convertir bookloans a BookLoanDto
            var bookLoansDto = mapper.Map<IEnumerable<BookLoanDto>>(bookLoans).ToList();

            // 1. Comprobar si hay estudiantes
            var studentsResponse = await studentBll.GetAll();
            if (studentsResponse.Result is IEnumerable<StudentDto> students)
            {
                var studentsDictionary = students.ToDictionary(s => s.StudentId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];
                    var studentId = studentIds[i];

                    if (studentsDictionary.TryGetValue(studentId, out var studentDto))
                    {
                        bookLoanDto.Student = studentDto;
                    }
                }
            }

            // 2. Comprobar si hay libros
            var booksResponse = await bookBll.GetAll();
            if (booksResponse.Result is IEnumerable<BookDto> books)
            {
                var booksDictionary = books.ToDictionary(b => b.BookId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];
                    var bookId = bookIds[i];

                    if (booksDictionary.TryGetValue(bookId, out var bookDto))
                    {
                        bookLoanDto.Book = bookDto;
                    }
                }
            }

            // 3. Borrowed users y Returned users
            var usersResponse = await userBll.GetAll();
            if (usersResponse.Result is IEnumerable<UserDto> users)
            {
                var usersDictionary = users.ToDictionary(u => u.UserId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];

                    var borrowedUserId = borrowedUserIds[i];
                    var returnedUserId = returnedUserIds[i];

                    if (usersDictionary.TryGetValue(borrowedUserId, out var borrowedUserDto))
                    {
                        bookLoanDto.BorrowedUser = borrowedUserDto;
                    }
                    if (usersDictionary.TryGetValue(returnedUserId, out var returnedUserDto))
                    {
                        bookLoanDto.ReturnedUser = returnedUserDto;
                    }
                }
            }

            // Retornar Dtos
            response.Result = bookLoansDto;

            return response;
        }

        public async Task<ApiResponse> GetBookLoanByTypeOfLoan(string? typeOfLoan)
        {
            var response = await repository.GetBookLoanByTypeOfLoan(typeOfLoan);
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<BookLoan> bookLoans)
                return response;

            // Obtener ids de los estudiantes de todos los prestamos
            var bookLoanList = bookLoans.ToList();
            var studentIds = bookLoanList.Select(b => b.StudentId).ToList();
            // Obtener ids de los libros de todos los prestamos
            var bookIds = bookLoanList.Select(b => b.BookId).ToList();
            // Obtener ids de los usuarios que aprobaron el prestamo del libro
            var borrowedUserIds = bookLoanList.Select(b => b.BorrowedUserId).ToList();
            // Obtener ids de los usuarios que recibieron el libro
            var returnedUserIds = bookLoanList.Select(b => b.ReturnedUserId).ToList();
            // Convertir bookloans a BookLoanDto
            var bookLoansDto = mapper.Map<IEnumerable<BookLoanDto>>(bookLoans).ToList();

            // 1. Comprobar si hay estudiantes
            var studentsResponse = await studentBll.GetAll();
            if (studentsResponse.Result is IEnumerable<StudentDto> students)
            {
                var studentsDictionary = students.ToDictionary(s => s.StudentId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];
                    var studentId = studentIds[i];

                    if (studentsDictionary.TryGetValue(studentId, out var studentDto))
                    {
                        bookLoanDto.Student = studentDto;
                    }
                }
            }

            // 2. Comprobar si hay libros
            var booksResponse = await bookBll.GetAll();
            if (booksResponse.Result is IEnumerable<BookDto> books)
            {
                var booksDictionary = books.ToDictionary(b => b.BookId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];
                    var bookId = bookIds[i];

                    if (booksDictionary.TryGetValue(bookId, out var bookDto))
                    {
                        bookLoanDto.Book = bookDto;
                    }
                }
            }

            // 3. Borrowed users y Returned users
            var usersResponse = await userBll.GetAll();
            if (usersResponse.Result is IEnumerable<UserDto> users)
            {
                var usersDictionary = users.ToDictionary(u => u.UserId);

                for (var i = 0; i < bookLoansDto.Count; i++)
                {
                    var bookLoanDto = bookLoansDto[i];

                    var borrowedUserId = borrowedUserIds[i];
                    var returnedUserId = returnedUserIds[i];

                    if (usersDictionary.TryGetValue(borrowedUserId, out var borrowedUserDto))
                    {
                        bookLoanDto.BorrowedUser = borrowedUserDto;
                    }
                    if (usersDictionary.TryGetValue(returnedUserId, out var returnedUserDto))
                    {
                        bookLoanDto.ReturnedUser = returnedUserDto;
                    }
                }
            }

            // Retornar Dtos
            response.Result = bookLoansDto;

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is not BookLoan bookLoan)
                return response;

            // Obtener id del estudiante que realizo el prestamo
            var studentId = bookLoan.StudentId;
            // Obtener id del libro que se presto
            var bookId = bookLoan.BookId;
            // Obtener id del usuario que presto el libro
            var borrowedUserId = bookLoan.BorrowedUserId;
            // Obtener id del usuario que recibio el libro
            var returnedUserId = bookLoan.ReturnedUserId;
            // Convertir bookloans a BookLoanDto
            var bookLoanDto = mapper.Map<BookLoanDto>(bookLoan);

            // 1. Comprobar si existe el estudiante (deberia...)
            var studentResponse = await studentBll.GetById(studentId);
            if (studentResponse.Result is StudentDto student)
            {
                bookLoanDto.Student = student;
            }

            // 2. Comprobar si existe el libro (deberia...)
            var bookResponse = await bookBll.GetById(bookId);
            if (bookResponse.Result is BookDto book)
            {
                bookLoanDto.Book = book;
            }

            // 3. Borrowed user y returned user
            var borrowedUserResponse = await userBll.GetById(borrowedUserId);
            if (borrowedUserResponse.Result is UserDto borrowedUserDto)
            {
                bookLoanDto.BorrowedUser = borrowedUserDto;
            }

            var returnedUserResponse = await userBll.GetById(returnedUserId);
            if (returnedUserResponse.Result is UserDto returnedUserDto)
            {
                bookLoanDto.ReturnedUser = returnedUserDto;
            }

            response.Result = bookLoanDto;

            return response;
        }

        public async Task<ApiResponse> UpdateBorrowedBookLoan(UpdateBorrowedBookLoanDto updateBorrowedBookLoanDto) =>
            await repository.UpdateBorrowedBookLoan(updateBorrowedBookLoanDto);

        public async Task<ApiResponse> UpdateReturnedBookLoan(UpdateReturnedBookLoanDto updateReturnedBookLoanDto) =>
            await repository.UpdateReturnedBookLoan(updateReturnedBookLoanDto);
    }
}