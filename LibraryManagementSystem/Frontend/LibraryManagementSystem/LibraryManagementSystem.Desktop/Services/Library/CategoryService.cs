using LibraryManagementSystem.Desktop.ApiConnection;
using LibraryManagementSystem.Desktop.Services.Base;
using LibraryManagementSystem.Entities.Dtos.Library;

namespace LibraryManagementSystem.Desktop.Services.Library
{
    public class CategoryService() : CreateUpdateManyRequestService<CategoryInsertDto, CategoryUpdateDto>(AppSettings.Instance.CategoryEndpointUrl)
    {
    }
}