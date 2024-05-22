using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface ISubCategoryBll : IBaseBll
    {
        Task<ApiResponse> Create(SubCategory entity);

        Task<ApiResponse> Update(SubCategory entity);

        Task<ApiResponse> Delete(int id);
        /* Para insertar varios registros a la vez */
        Task<ApiResponse> CreateMany(IEnumerable<SubCategory> entities);
        /* Para actualizar varios registros a la vez */
        Task<ApiResponse> UpdateMany(IEnumerable<SubCategory> entities);
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int id);
    }
}
