using LibraryManagementSystem.WinUi.Entities.Dtos.Library;
using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Library;

public class PublisherService : CreateUpdateManyRequestService<PublisherInsertDto, PublisherUpdateDto>
{
    public PublisherService() : base(AppSettings.Instance.PublisherEndpointUrl)
    {
    }
}