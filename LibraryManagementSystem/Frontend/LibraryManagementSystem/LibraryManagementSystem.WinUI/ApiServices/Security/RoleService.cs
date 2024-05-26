using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Entities.Dtos.Security;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Security;

public class RoleService : BaseRequestService<RoleInsertDto, RoleUpdateDto>
{
    public RoleService() : base(AppSettings.Instance.RoleEndpointUrl)
    {
    }
}