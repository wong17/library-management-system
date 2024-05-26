using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

using LibraryManagementSystem.WinUI.Contracts.ViewModels;
using LibraryManagementSystem.WinUI.Entities.Models.Library;
using LibraryManagementSystem.WinUI.Services.Library;

namespace LibraryManagementSystem.WinUI.ViewModels;

public partial class MonographViewModel : ObservableRecipient, INavigationAware
{
    private readonly MonographService _monographService;

    public ObservableCollection<Monograph> Source { get; } = new ObservableCollection<Monograph>();

    public MonographViewModel(MonographService monographService)
    {
        _monographService = monographService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Obtener monografías de la base de datos
        var response = await _monographService.Get();

        if (response is null || response.Result is null) { return; }

        if (response.Result is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
        {
            var array = JsonSerializer.Deserialize<IEnumerable<Monograph>>(jsonElement.GetRawText());
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
