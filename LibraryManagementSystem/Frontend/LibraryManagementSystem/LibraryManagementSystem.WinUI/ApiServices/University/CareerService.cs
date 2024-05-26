using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.University;

public class CareerService : GetRequestService
{
    public CareerService() : base(AppSettings.Instance.CareerEndpointUrl)
    {
    }
}