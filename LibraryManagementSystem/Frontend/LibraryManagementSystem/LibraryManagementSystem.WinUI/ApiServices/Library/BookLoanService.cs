using LibraryManagementSystem.WinUi.Entities.Dtos.Library;
using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Library;

public class BookLoanService : LoanRequestService<BookLoanInsertDto>
{
    public BookLoanService() : base(AppSettings.Instance.BookLoanEndpointUrl)
    {
    }
}