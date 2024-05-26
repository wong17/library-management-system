using System.Collections.ObjectModel;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using LibraryManagementSystem.WinUi.Entities.Models.Security;
using LibraryManagementSystem.WinUI.Contracts.ViewModels;
using LibraryManagementSystem.WinUI.Services.Security;

namespace LibraryManagementSystem.WinUI.ViewModels;

public partial class RoleViewModel : ObservableRecipient, INavigationAware
{
    private readonly RoleService _roleService;

    public ObservableCollection<Role> Source { get; } = new ObservableCollection<Role>();

    public RoleViewModel(RoleService roleService)
    {
        _roleService = roleService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Obtener roles de la base de datos
        var response = await _roleService.Get();

        if (response is null || response.Result is null) { return; }

        if (response.Result is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Array)
        {
            var array = JsonSerializer.Deserialize<IEnumerable<Role>>(jsonElement.GetRawText());
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
