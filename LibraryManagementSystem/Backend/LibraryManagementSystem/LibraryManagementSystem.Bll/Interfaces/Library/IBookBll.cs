using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Entities.Dtos.Library;
using LibraryManagementSystem.Entities.Models.Library;

namespace LibraryManagementSystem.Bll.Interfaces.Library
{
    public interface IBookBll : IBaseBll
    {
        Task<ApiResponse> Create(Book entity);

        Task<ApiResponse> Update(Book entity);
        
        Task<ApiResponse> Delete(int id);
        /* Para obtener todos los registros */
        Task<ApiResponse> GetAll();
        /* Para obtener un solo registro */
        Task<ApiResponse> GetById(int id);

        Task<ApiResponse> GetFilteredBook(FilterBookDto filterBookDto);
    }
}
