using LibraryManagementSystem.WinUi.Entities.Dtos.Library;
using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Library;

public class MonographAuthorService : CreateManyRequestService<MonographAuthorInsertDto>
{
    public MonographAuthorService() : base(AppSettings.Instance.MonographAuthorEndpointUrl)
    {
    }
}