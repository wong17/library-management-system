using LibraryManagementSystem.Desktop.ApiConnection;
using LibraryManagementSystem.Desktop.Services.Base;
using LibraryManagementSystem.Entities.Dtos.Security;

namespace LibraryManagementSystem.Desktop.Services.Security
{
    public class UserService() : BaseRequestService<UserInsertDto, UserUpdateDto>(AppSettings.Instance.UserEndpointUrl)
    { }
}