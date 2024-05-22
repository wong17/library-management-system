using LibraryManagementSystem.Common.Runtime;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Base
{
    public interface IRepositoryBase<in T> where T : class
    {
        Task<ApiResponse> Create(T entity);
        Task<ApiResponse> Update(T entity);
        Task<ApiResponse> Delete(int id);
    }
}
