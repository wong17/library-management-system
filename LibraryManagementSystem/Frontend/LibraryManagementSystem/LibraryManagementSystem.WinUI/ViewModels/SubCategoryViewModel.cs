using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

using LibraryManagementSystem.WinUI.Contracts.ViewModels;
using LibraryManagementSystem.WinUI.Entities.Models.Library;
using LibraryManagementSystem.WinUI.Services.Library;

namespace LibraryManagementSystem.WinUI.ViewModels;

public partial class SubCategoryViewModel : ObservableRecipient, INavigationAware
{
    private readonly SubCategoryService _subCategoryService;

    public ObservableCollection<SubCategory> Source { get; } = new ObservableCollection<SubCategory>();

    public SubCategoryViewModel(SubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Obtener sub categorías de la base de datos
        var response = await _subCategoryService.Get();

        if (response is null || response.Result is null) { return; }

        if (response.Result is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
        {
            var array = JsonSerializer.Deserialize<IEnumerable<SubCategory>>(jsonElement.GetRawText());
            if (array is null) { return; }

            foreach (var item in array)
            {
                Source.Add(item);
            }
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
