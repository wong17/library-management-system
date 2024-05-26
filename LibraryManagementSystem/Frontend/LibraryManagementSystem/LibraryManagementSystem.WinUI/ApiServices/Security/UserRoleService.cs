using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Entities.Dtos.Security;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Security;

public class UserRoleService : CreateManyRequestService<UserRoleInsertDto>
{
    public UserRoleService() : base(AppSettings.Instance.UserRoleEndpointUrl)
    {
    }
}