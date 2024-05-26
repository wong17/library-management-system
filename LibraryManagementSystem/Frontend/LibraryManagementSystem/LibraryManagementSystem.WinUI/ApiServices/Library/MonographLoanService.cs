using LibraryManagementSystem.WinUi.Entities.Dtos.Library;
using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Library;

public class MonographLoanService : LoanRequestService<MonographLoanInsertDto>
{
    public MonographLoanService() : base(AppSettings.Instance.MonographLoanEndpointUrl)
    {
    }
}