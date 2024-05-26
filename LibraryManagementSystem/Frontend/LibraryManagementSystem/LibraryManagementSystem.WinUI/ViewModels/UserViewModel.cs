using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using LibraryManagementSystem.WinUi.Entities.Models.Security;
using LibraryManagementSystem.WinUI.Contracts.ViewModels;
using LibraryManagementSystem.WinUI.Services.Security;

namespace LibraryManagementSystem.WinUI.ViewModels;

public partial class UserViewModel : ObservableRecipient, INavigationAware
{
    private readonly UserService _userService;

    public ObservableCollection<User> Source { get; } = new ObservableCollection<User>();

    public UserViewModel(UserService userService)
    {
        _userService = userService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();
        // Obtener usuarios de la base de datos
        var data = await _userService.Get();

        if (data is null || data.Result is null) { return; }
        if (data.Result is not IEnumerable<User> users) { return; }
            
        foreach (var item in users)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
