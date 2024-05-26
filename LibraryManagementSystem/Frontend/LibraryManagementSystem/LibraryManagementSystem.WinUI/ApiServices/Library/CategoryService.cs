﻿using LibraryManagementSystem.WinUi.Entities.Dtos.Library;
using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.Service.Base;

namespace LibraryManagementSystem.WinUI.Services.Library;

public class CategoryService : CreateUpdateManyRequestService<CategoryInsertDto, CategoryUpdateDto>
{
    public CategoryService() : base(AppSettings.Instance.CategoryEndpointUrl)
    {
    }
}