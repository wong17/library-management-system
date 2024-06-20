using LibraryManagementSystem.Common.Runtime;
using LibraryManagementSystem.Dal.Repository.Interfaces.Base;
using LibraryManagementSystem.Entities.Models.University;

namespace LibraryManagementSystem.Dal.Repository.Interfaces.University
{
    public interface IStudentRepository : IRepositoryGetAllBase<Student>
    {
        Task<ApiResponse> GetByCarnet(string? carnet);
    }
}
