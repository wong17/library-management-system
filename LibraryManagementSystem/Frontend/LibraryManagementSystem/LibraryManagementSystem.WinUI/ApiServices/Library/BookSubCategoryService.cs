using LibraryManagementSystem.WinUi.Entities.Dtos.Library;
using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Library;

public class BookSubCategoryService : CreateManyRequestService<BookSubCategoryInsertDto>
{
    public BookSubCategoryService() : base(AppSettings.Instance.BookSubCategoryEndpointUrl)
    {
    }
}