using LibraryManagementSystem.Desktop.ApiConnection;
using LibraryManagementSystem.Desktop.ApiConnection.Enums;

namespace LibraryManagementSystem.Desktop.Services.Base
{
    public abstract class GetRequestService
    {
        private readonly AppSettings _settings = AppSettings.Instance;
        private readonly string _uri;

        public GetRequestService(string? endpointUrl)
        {
            _uri = $"{_settings.UrlApi}/{endpointUrl}";
        }

        // GET
        public virtual async Task<ApiResponse> Get() => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, _uri);

        public virtual async Task<ApiResponse> GetById(int id) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, $"{_uri}/{id}");
    }
}