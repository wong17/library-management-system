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
    public class MonographLoanBll(IMonographLoanRepository repository, IMonographBll monographBll, IStudentBll studentBll, IMapper mapper) : IMonographLoanBll
    {
        private readonly IMonographLoanRepository _repository = repository;
        private readonly IMonographBll _monographBll = monographBll;
        private readonly IStudentBll _studentBll = studentBll;
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
            var studentIds = monographLoans.Select(s => s.StudentId).ToList();
            // Obtener ids de las monografias de todos los prestamos
            var monographIds = monographLoans.Select(b => b.MonographLoanId).ToList();
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

            response.Result = monographLoanDto;

            return response;
        }

        public async Task<ApiResponse> UpdateBorrowedMonographLoan(int monographLoanId, DateTime dueDate) =>
            await _repository.UpdateBorrowedMonographLoan(monographLoanId, dueDate);

        public async Task<ApiResponse> UpdateReturnedMonographLoan(int monographLoanId) => await _repository.UpdateReturnedMonographLoan(monographLoanId);
    }
}
