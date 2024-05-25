using LibraryManagementSystem.Desktop.ApiConnection;
using LibraryManagementSystem.Desktop.Services.Base;

namespace LibraryManagementSystem.Desktop.Services.University
{
    public class CareerService() : GetRequestService(AppSettings.Instance.CareerEndpointUrl)
    { }
}