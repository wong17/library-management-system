using LibraryManagementSystem.WinUI.ViewModels;
using Microsoft.UI.Xaml.Controls;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace LibraryManagementSystem.WinUI.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class RolePage : Page
{
    public RoleViewModel ViewModel
    {
        get;
    }

    public RolePage()
    {
        ViewModel = App.GetService<RoleViewModel>();
        InitializeComponent();
    }
}
