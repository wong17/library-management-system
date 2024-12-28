using LibraryManagementSystem.Bll.Interfaces.Base;
using LibraryManagementSystem.Common.Runtime;

namespace LibraryManagementSystem.Bll.Interfaces.University
{
    public interface ICareerBll : IBaseBll
    {
        /* Para obtener todos los registros */

        Task<ApiResponse> GetAll();

        /* Para obtener un solo registro */

        Task<ApiResponse> GetById(int id);
    }
}