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
        private readonly IMonographLoanRepository _repository = repository;
        private readonly IMonographBll _monographBll = monographBll;
        private readonly IStudentBll _studentBll = studentBll;
        private readonly IUserBll _userBll = userBll;
        private readonly IMapper _mapper = mapper;

        public async Task<ApiResponse> Create(MonographLoan entity) => await _repository.Create(entity);

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll()
        {
            var response = await _repository.GetAll();
            // Comprobar si hay elementos
            if (response.Result is null || response.Result is not IEnumerable<MonographLoan> monographLoans)
                return response;

            // Obtener ids de los estudiantes de todos los prestamos
            var studentIds = monographLoans.Select(m => m.StudentId).ToList();
            // Obtener ids de las monografias de todos los prestamos
            var monographIds = monographLoans.Select(m => m.MonographLoanId).ToList();
            // Obtener ids de los usuarios que aprobaron el prestamo del libro
            var borrowedUserIds = monographLoans.Select(m => m.BorrowedUserId).ToList();
            // Obtener ids de los usuarios que recibieron el libro
            var returnedUserIds = monographLoans.Select(m => m.ReturnedUserId).ToList();
            // Convertir bookloans a BookLoanDto
            var monographLoansDto = _mapper.Map<IEnumerable<MonographLoanDto>>(monographLoans).ToList();

            // 1. Comprobar si hay estudiantes
            var studentsResponse = await _studentBll.GetAll();
            if (studentsResponse.Result is not null && studentsResponse.Result is IEnumerable<StudentDto> students)
            {
                var studentsDictionary = students.ToDictionary(s => s.StudentId);

                for (int i = 0; i < monographLoansDto.Count; i++)
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
            var monographsResponse = await _monographBll.GetAll();
            if (monographsResponse.Result is not null && monographsResponse.Result is IEnumerable<MonographDto> monographs)
            {
                var monographsDictionary = monographs.ToDictionary(m => m.MonographId);

                for (int i = 0; i < monographLoansDto.Count; i++)
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
            var usersResponse = await _userBll.GetAll();
            if (usersResponse.Result is not null && usersResponse.Result is IEnumerable<UserDto> users)
            {
                var usersDictionary = users.ToDictionary(u => u.UserId);

                for (int i = 0; i < monographLoansDto.Count; i++)
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
            var response = await _repository.GetById(id);
            // Comprobar si hay un elemento
            if (response.Result is null || response.Result is not MonographLoan monographLoan)
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
            var monographLoanDto = _mapper.Map<MonographLoanDto>(monographLoan);

            // 1. Comprobar si existe el estudiante (deberia...)
            var studentResponse = await _studentBll.GetById(studentId);
            if (studentResponse.Result is not null && studentResponse.Result is StudentDto student)
            {
                monographLoanDto.Student = student;
            }

            // 2. Comprobar si existe el libro (deberia...)
            var monographResponse = await _monographBll.GetById(monographId);
            if (monographResponse.Result is not null && monographResponse.Result is MonographDto monograph)
            {
                monographLoanDto.Monograph = monograph;
            }

            // 3. Borrowed user y returned user
            var borrowedUserResponse = await _userBll.GetById(borrowedUserId);
            if (borrowedUserResponse.Result is not null && borrowedUserResponse.Result is UserDto borrowedUserDto)
            {
                monographLoanDto.BorrowedUser = borrowedUserDto;
            }

            var returnedUserResponse = await _userBll.GetById(returnedUserId);
            if (returnedUserResponse.Result is not null && returnedUserResponse.Result is UserDto returnedUserDto)
            {
                monographLoanDto.ReturnedUser = returnedUserDto;
            }

            response.Result = monographLoanDto;

            return response;
        }

        public async Task<ApiResponse> UpdateBorrowedMonographLoan(UpdateBorrowedMonographLoanDto updateBorrowedMonographLoanDto) =>
            await _repository.UpdateBorrowedMonographLoan(updateBorrowedMonographLoanDto);

        public async Task<ApiResponse> UpdateReturnedMonographLoan(UpdateReturnedMonographLoanDto updateReturnedMonographLoanDto) => 
            await _repository.UpdateReturnedMonographLoan(updateReturnedMonographLoanDto);
    }
}
