using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.ApiConnection.Enums;

namespace LibraryManagementSystem.WinUI.Service.Base;

public abstract class LoanRequestService<TCreateDto> where TCreateDto : class, new()
{
    private readonly AppSettings _settings = AppSettings.Instance;
    protected readonly string Uri;

    public LoanRequestService(string? endpointUrl)
    {
        Uri = $"{_settings.UrlApi}{endpointUrl}";
    }

    // GET
    public async virtual Task<ApiResponse> Get() => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, Uri);

    public async virtual Task<ApiResponse> GetById(int id) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, $"{Uri}/{id}");

    // POST
    public async virtual Task<ApiResponse> Create(TCreateDto createDto) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Post, Uri, createDto);

    // PUT
    public async virtual Task<ApiResponse> UpdateBorrowedLoan(int loanId, DateTime dueDate) =>
        await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Put, $"{Uri}/{loanId}/{dueDate}");

    public async virtual Task<ApiResponse> UpdateReturnedLoan(int loanId) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Put, $"{Uri}/{loanId}");
}