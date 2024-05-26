using LibraryManagementSystem.WinUI.Core.Models;

namespace LibraryManagementSystem.WinUI.Core.Contracts.Services;

// Remove this class once your pages/features are using your data.
public interface ISampleDataService
{
    Task<IEnumerable<SampleOrder>> GetGridDataAsync();
}
