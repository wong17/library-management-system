using LibraryManagementSystem.Desktop.ApiConnection;
using LibraryManagementSystem.Desktop.ApiConnection.Enums;
namespace LibraryManagementSystem.Desktop.Services.Base
{
    public abstract class BaseRequestService <TCreateDto, TUpdateDto> where TCreateDto : class, new () where TUpdateDto : class, new ()
    {
        private readonly AppSettings _settings = AppSettings.Instance;
        protected readonly string Uri;

        public BaseRequestService(string? endpointUrl)
        {
            Uri = $"{_settings.UrlApi}/{endpointUrl}";
        }

        // GET
        public virtual async Task<ApiResponse> Get() => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, Uri);

        public virtual async Task<ApiResponse> GetById(int id) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, $"{Uri}/{id}");

        // POST
        public virtual async Task<ApiResponse> Create(TCreateDto createDto) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Post, Uri, createDto);

        // PUT
        public virtual async Task<ApiResponse> Update(TUpdateDto updateDto) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Put, Uri, updateDto);

        // DELETE
        public virtual async Task<ApiResponse> Delete(int id) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Delete, $"{Uri}/{id}");
    }
}
