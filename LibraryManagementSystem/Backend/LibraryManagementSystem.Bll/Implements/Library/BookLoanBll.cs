using LibraryManagementSystem.Bll.Interfaces.Library;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Implements.Library
{
    public class BookLoanBll(IBookLoanRepository repository) : IBookLoanBll
    {
        private readonly IBookLoanRepository _repository = repository;

        public async Task<ApiResponse> Create(BookLoan entity) => await _repository.Create(entity);

        public async Task<ApiResponse> Delete(int id) => await _repository.Delete(id);

        public async Task<ApiResponse> GetAll() => await _repository.GetAll();

        public async Task<ApiResponse> GetById(int id) => await _repository.GetById(id);

        public async Task<ApiResponse> UpdateBorrowedBookLoan(int bookLoanId, DateTime dueDate) => await _repository.UpdateBorrowedBookLoan(bookLoanId, dueDate);

        public async Task<ApiResponse> UpdateReturnedBookLoan(int bookLoanId) => await _repository.UpdateReturnedBookLoan(bookLoanId);
    }
}
