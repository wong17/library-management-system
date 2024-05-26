using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using LibraryManagementSystem.WinUI.Contracts.ViewModels;
using LibraryManagementSystem.WinUI.Entities.Models.Library;
using LibraryManagementSystem.WinUI.Services.Library;

namespace LibraryManagementSystem.WinUI.ViewModels;

public partial class PublisherViewModel : ObservableRecipient, INavigationAware
{
    private readonly PublisherService _publisherService;

    public ObservableCollection<Publisher> Source { get; } = new ObservableCollection<Publisher>();

    public PublisherViewModel(PublisherService publisherService)
    {
        _publisherService = publisherService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Obtener editoriales de la base de datos
        var response = await _publisherService.Get();

        if (response is null || response.Result is null) { return; }

        if (response.Result is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
        {
            var array = JsonSerializer.Deserialize<IEnumerable<Publisher>>(jsonElement.GetRawText());
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
