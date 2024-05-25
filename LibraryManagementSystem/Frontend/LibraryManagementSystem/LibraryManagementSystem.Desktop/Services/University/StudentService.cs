using LibraryManagementSystem.Desktop.ApiConnection;
using LibraryManagementSystem.Desktop.Services.Base;

namespace LibraryManagementSystem.Desktop.Services.University
{
    public class StudentService() : GetRequestService(AppSettings.Instance.StudentEndpointUrl)
    { }
}