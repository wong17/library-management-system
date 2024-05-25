using LibraryManagementSystem.Desktop.ApiConnection;
using LibraryManagementSystem.Desktop.Services.Base;
using LibraryManagementSystem.Entities.Dtos.Security;

namespace LibraryManagementSystem.Desktop.Services.Security
{
    public class RoleService() : BaseRequestService<RoleInsertDto, RoleUpdateDto>(AppSettings.Instance.RoleEndpointUrl)
    {
    }
}