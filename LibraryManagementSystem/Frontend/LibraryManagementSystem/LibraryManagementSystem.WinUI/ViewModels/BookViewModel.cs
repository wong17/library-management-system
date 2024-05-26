using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

using LibraryManagementSystem.WinUI.Contracts.ViewModels;
using LibraryManagementSystem.WinUI.Entities.Models.Library;
using LibraryManagementSystem.WinUI.Services.Library;

namespace LibraryManagementSystem.WinUI.ViewModels;

public partial class BookViewModel : ObservableRecipient, INavigationAware
{
    private readonly BookService _bookService;

    public ObservableCollection<Book> Source { get; } = new ObservableCollection<Book>();

    public BookViewModel(BookService bookService)
    {
        _bookService = bookService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Obtener libros de la base de datos
        var response = await _bookService.Get();

        if (response is null || response.Result is null) { return; }

        if (response.Result is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
        {
            var array = JsonSerializer.Deserialize<IEnumerable<Book>>(jsonElement.GetRawText());
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
