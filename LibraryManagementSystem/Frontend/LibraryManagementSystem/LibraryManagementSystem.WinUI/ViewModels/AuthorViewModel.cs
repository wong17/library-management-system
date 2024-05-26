using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

using LibraryManagementSystem.WinUI.Contracts.ViewModels;
using LibraryManagementSystem.WinUI.Entities.Models.Library;
using LibraryManagementSystem.WinUI.Services.Library;

namespace LibraryManagementSystem.WinUI.ViewModels;

public partial class AuthorViewModel : ObservableRecipient, INavigationAware
{
    private readonly AuthorService _authorService;

    public ObservableCollection<Author> Source { get; } = new ObservableCollection<Author>();

    public AuthorViewModel(AuthorService authorService)
    {
        _authorService = authorService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Obtener autores de la base de datos
        var response = await _authorService.Get();

        if (response is null || response.Result is null) { return; }

        if (response.Result is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array) 
        {
            var array = JsonSerializer.Deserialize<IEnumerable<Author>>(jsonElement.GetRawText());
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
