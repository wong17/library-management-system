using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.ApiConnection.Enums;

namespace LibraryManagementSystem.WinUI.Service.Base;

public abstract class GetRequestService
{
    private readonly AppSettings _settings = AppSettings.Instance;
    private readonly string _uri;

    public GetRequestService(string? endpointUrl)
    {
        _uri = $"{_settings.UrlApi}{endpointUrl}";
    }

    // GET
    public async virtual Task<ApiResponse> Get() => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, _uri);

    public async virtual Task<ApiResponse> GetById(int id) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, $"{_uri}/{id}");
}