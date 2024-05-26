using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

using LibraryManagementSystem.WinUI.Contracts.ViewModels;
using LibraryManagementSystem.WinUI.Entities.Models.Library;
using LibraryManagementSystem.WinUI.Services.Library;

namespace LibraryManagementSystem.WinUI.ViewModels;

public partial class CategoryViewModel : ObservableRecipient, INavigationAware
{
    private readonly CategoryService _categoryService;

    public ObservableCollection<Category> Source { get; } = new ObservableCollection<Category>();

    public CategoryViewModel(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Obtener categorías de la base de datos
        var response = await _categoryService.Get();

        if (response is null || response.Result is null) { return; }

        if (response.Result is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
        {
            var array = JsonSerializer.Deserialize<IEnumerable<Category>>(jsonElement.GetRawText());
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
