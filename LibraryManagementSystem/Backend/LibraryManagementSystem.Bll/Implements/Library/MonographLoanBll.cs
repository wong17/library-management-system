using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class MonographLoanBll(IMonographLoanRepository repository) : IMonographLoanBll
    {
        private readonly IMonographLoanRepository _repository = repository;

        public async Task<ApiResponse> Create(MonographLoan entity) => await _repository.Create(entity);

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll() => await _repository.GetAll();

        public async Task<ApiResponse> GetById(int id) => await _repository.GetById(id);

        public async Task<ApiResponse> UpdateBorrowedMonographLoan(int monographLoanId, DateTime dueDate) =>
            await _repository.UpdateBorrowedMonographLoan(monographLoanId, dueDate);

        public async Task<ApiResponse> UpdateReturnedMonographLoan(int monographLoanId) => await _repository.UpdateReturnedMonographLoan(monographLoanId);
    }
}
