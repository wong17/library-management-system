using LibraryManagementSystem.Desktop.ApiConnection;
using LibraryManagementSystem.Desktop.Services.Base;
using LibraryManagementSystem.Entities.Dtos.Library;

namespace LibraryManagementSystem.Desktop.Services.Library
{
    public class BookAuthorService() : CreateManyRequestService<BookAuthorInsertDto>(AppSettings.Instance.BookAuthorEndpointUrl)
    {
    }
}