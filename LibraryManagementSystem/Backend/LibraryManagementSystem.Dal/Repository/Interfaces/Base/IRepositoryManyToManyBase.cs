using LibraryManagementSystem.Common.Runtime;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.Base
{
    public interface IRepositoryManyToManyBase<in T> where T : class
    {
        Task<ApiResponse> Create(T entity);
        Task<ApiResponse> Delete(int id1, int id2);
        Task<ApiResponse> CreateMany(IEnumerable<T> entities);
        Task<ApiResponse> GetAll();
        Task<ApiResponse> GetById(int id1, int id2);
    }
}
