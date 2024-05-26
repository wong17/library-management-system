using LibraryManagementSystem.WinUi.Entities.Dtos.Library;
using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Library;

public class BookService : BaseRequestService<BookInsertDto, BookUpdateDto>
{
    public BookService() : base(AppSettings.Instance.BookEndpointUrl)
    {
    }
}