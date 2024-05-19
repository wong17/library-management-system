using LibraryManagementSystem.Common.Runtime;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Base
{
    public interface IRepositoryLoanBase<in T> where T : class
    {
        Task<ApiResponse> Create(T entity);
        Task<ApiResponse> Delete(int id);
    }
}
