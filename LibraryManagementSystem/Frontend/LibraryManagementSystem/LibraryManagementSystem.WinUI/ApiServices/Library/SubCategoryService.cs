using LibraryManagementSystem.WinUi.Entities.Dtos.Library;
using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Library;

public class SubCategoryService : CreateUpdateManyRequestService<SubCategoryInsertDto, SubCategoryUpdateDto>
{
    public SubCategoryService() : base(AppSettings.Instance.SubCategoryEndpointUrl)
    {
    }
}