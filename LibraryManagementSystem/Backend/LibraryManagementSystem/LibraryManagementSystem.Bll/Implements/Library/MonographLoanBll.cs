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
    public class MonographLoanBll(IMonographLoanRepository repository, IMonographBll monographBll, IStudentBll studentBll,
        IUserBll userBll, IMapper mapper) : IMonographLoanBll
    {
        public async Task<ApiResponse> Create(MonographLoanInsertDto entity) => await repository.Create(mapper.Map<MonographLoan>(entity));

        public async Task<ApiResponse> Delete(int id) => await repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<MonographLoan> monographLoans)
                return response;

            // Obtener ids de los estudiantes de todos los prestamos
            var monographLoanList = monographLoans.ToList();
            var studentIds = monographLoanList.Select(m => m.StudentId).ToList();
            // Obtener ids de las monografias de todos los prestamos
            var monographIds = monographLoanList.Select(m => m.MonographId).ToList();
            // Obtener ids de los usuarios que aprobaron el prestamo del libro
            var borrowedUserIds = monographLoanList.Select(m => m.BorrowedUserId).ToList();
            // Obtener ids de los usuarios que recibieron el libro
            var returnedUserIds = monographLoanList.Select(m => m.ReturnedUserId).ToList();
            // Convertir bookloans a BookLoanDto
            var monographLoansDto = mapper.Map<IEnumerable<MonographLoanDto>>(monographLoans).ToList();

            // 1. Comprobar si hay estudiantes
            var studentsResponse = await studentBll.GetAll();
            if (studentsResponse.Result is IEnumerable<StudentDto> students)
            {
                var studentsDictionary = students.ToDictionary(s => s.StudentId);

                for (var i = 0; i < monographLoansDto.Count; i++)
                {
                    var bookLoanDto = monographLoansDto[i];
                    var studentId = studentIds[i];

                    if (studentsDictionary.TryGetValue(studentId, out var studentDto))
                    {
                        bookLoanDto.Student = studentDto;
                    }
                }
            }

            // 2. Comprobar si hay monografias
            var monographsResponse = await monographBll.GetAll();
            if (monographsResponse.Result is IEnumerable<MonographDto> monographs)
            {
                var monographsDictionary = monographs.ToDictionary(m => m.MonographId);

                for (var i = 0; i < monographLoansDto.Count; i++)
                {
                    var monographLoanDto = monographLoansDto[i];
                    var monographId = monographIds[i];

                    if (monographsDictionary.TryGetValue(monographId, out var monographDto))
                    {
                        monographLoanDto.Monograph = monographDto;
                    }
                }
            }

            // 3. Borrowed users y Returned users
            var usersResponse = await userBll.GetAll();
            if (usersResponse.Result is IEnumerable<UserDto> users)
            {
                var usersDictionary = users.ToDictionary(u => u.UserId);

                for (var i = 0; i < monographLoansDto.Count; i++)
                {
                    var monographLoanDto = monographLoansDto[i];

                    var borrowedUserId = borrowedUserIds[i];
                    var returnedUserId = returnedUserIds[i];

                    if (usersDictionary.TryGetValue(borrowedUserId, out var borrowedUserDto))
                    {
                        monographLoanDto.BorrowedUser = borrowedUserDto;
                    }
                    if (usersDictionary.TryGetValue(returnedUserId, out var returnedUserDto))
                    {
                        monographLoanDto.ReturnedUser = returnedUserDto;
                    }
                }
            }

            response.Result = monographLoansDto;

            return response;
        }

        public async Task<ApiResponse> GetById(int id)
        {
            var response = await repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is not MonographLoan monographLoan)
                return response;

            // Obtener id del estudiante que realizo el prestamo
            var studentId = monographLoan.StudentId;
            // Obtener id de la monografia que se presto
            var monographId = monographLoan.MonographLoanId;
            // Obtener id del usuario que presto el libro
            var borrowedUserId = monographLoan.BorrowedUserId;
            // Obtener id del usuario que recibio el libro
            var returnedUserId = monographLoan.ReturnedUserId;
            // Convertir monographLoan a MonographLoanDto
            var monographLoanDto = mapper.Map<MonographLoanDto>(monographLoan);

            // 1. Comprobar si existe el estudiante (deberia...)
            var studentResponse = await studentBll.GetById(studentId);
            if (studentResponse.Result is StudentDto student)
            {
                monographLoanDto.Student = student;
            }

            // 2. Comprobar si existe el libro (deberia...)
            var monographResponse = await monographBll.GetById(monographId);
            if (monographResponse.Result is MonographDto monograph)
            {
                monographLoanDto.Monograph = monograph;
            }

            // 3. Borrowed user y returned user
            var borrowedUserResponse = await userBll.GetById(borrowedUserId);
            if (borrowedUserResponse.Result is UserDto borrowedUserDto)
            {
                monographLoanDto.BorrowedUser = borrowedUserDto;
            }

            var returnedUserResponse = await userBll.GetById(returnedUserId);
            if (returnedUserResponse.Result is UserDto returnedUserDto)
            {
                monographLoanDto.ReturnedUser = returnedUserDto;
            }

            response.Result = monographLoanDto;

            return response;
        }

        public async Task<ApiResponse> GetMonographLoanByState(string? state)
        {
            var response = await repository.GetMonographLoanByState(state);
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<MonographLoan> monographLoans)
                return response;

            // Obtener ids de los estudiantes de todos los prestamos
            var monographLoanList = monographLoans.ToList();
            var studentIds = monographLoanList.Select(m => m.StudentId).ToList();
            // Obtener ids de las monografias de todos los prestamos
            var monographIds = monographLoanList.Select(m => m.MonographId).ToList();
            // Obtener ids de los usuarios que aprobaron el prestamo del libro
            var borrowedUserIds = monographLoanList.Select(m => m.BorrowedUserId).ToList();
            // Obtener ids de los usuarios que recibieron el libro
            var returnedUserIds = monographLoanList.Select(m => m.ReturnedUserId).ToList();
            // Convertir bookloans a BookLoanDto
            var monographLoansDto = mapper.Map<IEnumerable<MonographLoanDto>>(monographLoans).ToList();

            // 1. Comprobar si hay estudiantes
            var studentsResponse = await studentBll.GetAll();
            if (studentsResponse.Result is IEnumerable<StudentDto> students)
            {
                var studentsDictionary = students.ToDictionary(s => s.StudentId);

                for (var i = 0; i < monographLoansDto.Count; i++)
                {
                    var bookLoanDto = monographLoansDto[i];
                    var studentId = studentIds[i];

                    if (studentsDictionary.TryGetValue(studentId, out var studentDto))
                    {
                        bookLoanDto.Student = studentDto;
                    }
                }
            }

            // 2. Comprobar si hay monografias
            var monographsResponse = await monographBll.GetAll();
            if (monographsResponse.Result is IEnumerable<MonographDto> monographs)
            {
                var monographsDictionary = monographs.ToDictionary(m => m.MonographId);

                for (var i = 0; i < monographLoansDto.Count; i++)
                {
                    var monographLoanDto = monographLoansDto[i];
                    var monographId = monographIds[i];

                    if (monographsDictionary.TryGetValue(monographId, out var monographDto))
                    {
                        monographLoanDto.Monograph = monographDto;
                    }
                }
            }

            // 3. Borrowed users y Returned users
            var usersResponse = await userBll.GetAll();
            if (usersResponse.Result is IEnumerable<UserDto> users)
            {
                var usersDictionary = users.ToDictionary(u => u.UserId);

                for (var i = 0; i < monographLoansDto.Count; i++)
                {
                    var monographLoanDto = monographLoansDto[i];

                    var borrowedUserId = borrowedUserIds[i];
                    var returnedUserId = returnedUserIds[i];

                    if (usersDictionary.TryGetValue(borrowedUserId, out var borrowedUserDto))
                    {
                        monographLoanDto.BorrowedUser = borrowedUserDto;
                    }
                    if (usersDictionary.TryGetValue(returnedUserId, out var returnedUserDto))
                    {
                        monographLoanDto.ReturnedUser = returnedUserDto;
                    }
                }
            }

            response.Result = monographLoansDto;

            return response;
        }

        public async Task<ApiResponse> GetMonographLoanByStudentCarnet(string? carnet)
        {
            var response = await repository.GetMonographLoanByStudentCarnet(carnet);
            // Comprobar si hay elementos
            if (response.Result is not IEnumerable<MonographLoan> monographLoans)
                return response;

            // Obtener ids de los estudiantes de todos los prestamos
            var monographLoanList = monographLoans.ToList();
            var studentIds = monographLoanList.Select(m => m.StudentId).ToList();
            // Obtener ids de las monografias de todos los prestamos
            var monographIds = monographLoanList.Select(m => m.MonographId).ToList();
            // Obtener ids de los usuarios que aprobaron el prestamo del libro
            var borrowedUserIds = monographLoanList.Select(m => m.BorrowedUserId).ToList();
            // Obtener ids de los usuarios que recibieron el libro
            var returnedUserIds = monographLoanList.Select(m => m.ReturnedUserId).ToList();
            // Convertir bookloans a BookLoanDto
            var monographLoansDto = mapper.Map<IEnumerable<MonographLoanDto>>(monographLoans).ToList();

            // 1. Comprobar si hay estudiantes
            var studentsResponse = await studentBll.GetAll();
            if (studentsResponse.Result is IEnumerable<StudentDto> students)
            {
                var studentsDictionary = students.ToDictionary(s => s.StudentId);

                for (var i = 0; i < monographLoansDto.Count; i++)
                {
                    var bookLoanDto = monographLoansDto[i];
                    var studentId = studentIds[i];

                    if (studentsDictionary.TryGetValue(studentId, out var studentDto))
                    {
                        bookLoanDto.Student = studentDto;
                    }
                }
            }

            // 2. Comprobar si hay monografias
            var monographsResponse = await monographBll.GetAll();
            if (monographsResponse.Result is IEnumerable<MonographDto> monographs)
            {
                var monographsDictionary = monographs.ToDictionary(m => m.MonographId);

                for (var i = 0; i < monographLoansDto.Count; i++)
                {
                    var monographLoanDto = monographLoansDto[i];
                    var monographId = monographIds[i];

                    if (monographsDictionary.TryGetValue(monographId, out var monographDto))
                    {
                        monographLoanDto.Monograph = monographDto;
                    }
                }
            }

            // 3. Borrowed users y Returned users
            var usersResponse = await userBll.GetAll();
            if (usersResponse.Result is IEnumerable<UserDto> users)
            {
                var usersDictionary = users.ToDictionary(u => u.UserId);

                for (var i = 0; i < monographLoansDto.Count; i++)
                {
                    var monographLoanDto = monographLoansDto[i];

                    var borrowedUserId = borrowedUserIds[i];
                    var returnedUserId = returnedUserIds[i];

                    if (usersDictionary.TryGetValue(borrowedUserId, out var borrowedUserDto))
                    {
                        monographLoanDto.BorrowedUser = borrowedUserDto;
                    }
                    if (usersDictionary.TryGetValue(returnedUserId, out var returnedUserDto))
                    {
                        monographLoanDto.ReturnedUser = returnedUserDto;
                    }
                }
            }

            response.Result = monographLoansDto;

            return response;
        }

        public async Task<ApiResponse> UpdateBorrowedMonographLoan(UpdateBorrowedMonographLoanDto updateBorrowedMonographLoanDto) =>
            await repository.UpdateBorrowedMonographLoan(updateBorrowedMonographLoanDto);

        public async Task<ApiResponse> UpdateReturnedMonographLoan(UpdateReturnedMonographLoanDto updateReturnedMonographLoanDto) =>
            await repository.UpdateReturnedMonographLoan(updateReturnedMonographLoanDto);
    }
}