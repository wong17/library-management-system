using LibraryManagementSystem.Bll.Interfaces.University;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.University;

namespace LibraryManagementSystem.Bll.Implements.University
{
    public class StudentBll(IStudentRepository repository) : IStudentBll
    {
        private readonly IStudentRepository _repository = repository;

        public async Task<ApiResponse> GetAll() => await _repository.GetAll();

        public async Task<ApiResponse> GetById(int id) => await _repository.GetById(id);
    }
}
