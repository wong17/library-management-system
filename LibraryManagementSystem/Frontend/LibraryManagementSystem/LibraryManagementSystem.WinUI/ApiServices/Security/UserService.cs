using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Entities.Dtos.Security;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Security;

public class UserService : BaseRequestService<UserInsertDto, UserUpdateDto>
{
    public UserService() : base(AppSettings.Instance.UserEndpointUrl)
    {
    }
}