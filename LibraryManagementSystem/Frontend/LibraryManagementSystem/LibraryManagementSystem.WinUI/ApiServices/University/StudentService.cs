using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.University;

public class StudentService : GetRequestService
{
    public StudentService() : base(AppSettings.Instance.StudentEndpointUrl)
    {
    }
}