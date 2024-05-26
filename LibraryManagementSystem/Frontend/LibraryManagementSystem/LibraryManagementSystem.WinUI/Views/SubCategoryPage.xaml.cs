using LibraryManagementSystem.WinUI.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace LibraryManagementSystem.WinUI.Views;

// TODO: Change the grid as appropriate for your app. Adjust the column definitions on DataGridPage.xaml.
// For more details, see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid.
public sealed partial class SubCategoryPage : Page
{
    public SubCategoryViewModel ViewModel
    {
        get;
    }

    public SubCategoryPage()
    {
        ViewModel = App.GetService<SubCategoryViewModel>();
        InitializeComponent();
    }
}
