using LibraryManagementSystem.WinUi.Entities.Dtos.Library;
using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Library;

public class BookAuthorService : CreateManyRequestService<BookAuthorInsertDto>
{
    public BookAuthorService() : base(AppSettings.Instance.BookAuthorEndpointUrl)
    {
    }
}