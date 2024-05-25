using LibraryManagementSystem.Desktop.ApiConnection;
using LibraryManagementSystem.Entities.Dtos.Security;
using LibraryManagementSystem.Desktop.Services.Base;

namespace LibraryManagementSystem.Desktop.Services.Security
{
    public class UserRoleService() : CreateManyRequestService<UserRoleInsertDto>(AppSettings.Instance.UserRoleEndpointUrl)
    {
    }
}