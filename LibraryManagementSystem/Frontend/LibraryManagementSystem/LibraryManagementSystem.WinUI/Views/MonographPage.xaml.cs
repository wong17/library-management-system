using LibraryManagementSystem.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace LibraryManagementSystem.WinUI.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class MonographPage : Page
{
    public MonographViewModel ViewModel
    {
        get;
    }

    public MonographPage()
    {
        ViewModel = App.GetService<MonographViewModel>();
        InitializeComponent();
    }
}
