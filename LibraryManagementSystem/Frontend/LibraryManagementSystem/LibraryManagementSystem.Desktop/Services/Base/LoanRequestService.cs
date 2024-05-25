using LibraryManagementSystem.Desktop.ApiConnection.Enums;
using LibraryManagementSystem.Desktop.ApiConnection;

namespace LibraryManagementSystem.Desktop.Services.Base
{
    public abstract class LoanRequestService <TCreateDto> where TCreateDto : class, new()
    {
        private readonly AppSettings _settings = AppSettings.Instance;
        protected readonly string Uri;

        public LoanRequestService(string? endpointUrl)
        {
            Uri = $"{_settings.UrlApi}/{endpointUrl}";
        }

        // GET
        public virtual async Task<ApiResponse> Get() => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, Uri);

        public virtual async Task<ApiResponse> GetById(int id) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, $"{Uri}/{id}");

        // POST
        public virtual async Task<ApiResponse> Create(TCreateDto createDto) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Post, Uri, createDto);

        // PUT
        public virtual async Task<ApiResponse> UpdateBorrowedLoan(int loanId, DateTime dueDate) => 
            await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Put, $"{Uri}/{loanId}/{dueDate}");

        public virtual async Task<ApiResponse> UpdateReturnedLoan(int loanId) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Put, $"{Uri}/{loanId}");
    }
}
