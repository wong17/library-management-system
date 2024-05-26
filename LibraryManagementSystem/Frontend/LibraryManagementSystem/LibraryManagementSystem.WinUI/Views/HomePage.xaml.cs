using LibraryManagementSystem.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace LibraryManagementSystem.WinUI.Views;

public sealed partial class HomePage : Page
{
    public HomeViewModel ViewModel
    {
        get;
    }

    public HomePage()
    {
        ViewModel = App.GetService<HomeViewModel>();
        InitializeComponent();
    }
}
