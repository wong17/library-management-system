using LibraryManagementSystem.Bll.Interfaces;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Bll.Implements
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
