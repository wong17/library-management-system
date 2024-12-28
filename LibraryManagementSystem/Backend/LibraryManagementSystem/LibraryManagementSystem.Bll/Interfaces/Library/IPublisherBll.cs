using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface IPublisherBll : IBaseBll
    {
        Task<ApiResponse> Create(PublisherInsertDto entity);

        Task<ApiResponse> Update(PublisherUpdateDto entity);

        Task<ApiResponse> Delete(int id);

        /* Para insertar varios registros a la vez */

        Task<ApiResponse> CreateMany(IEnumerable<PublisherInsertDto> entities);

        /* Para actualizar varios registros a la vez */

        Task<ApiResponse> UpdateMany(IEnumerable<PublisherUpdateDto> entities);

        /* Para obtener todos los registros */

        Task<ApiResponse> GetAll();

        /* Para obtener un solo registro */

        Task<ApiResponse> GetById(int id);
    }
}