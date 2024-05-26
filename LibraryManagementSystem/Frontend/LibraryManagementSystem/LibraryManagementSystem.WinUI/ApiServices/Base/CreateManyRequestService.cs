using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.ApiConnection.Enums;

namespace LibraryManagementSystem.WinUI.Service.Base;

public abstract class CreateManyRequestService<TCreateDto> where TCreateDto : class, new()
{
    private readonly AppSettings _settings = AppSettings.Instance;
    protected readonly string Uri;

    public CreateManyRequestService(string? endpointUrl)
    {
        Uri = $"{_settings.UrlApi}{endpointUrl}";
    }

    // GET
    public async virtual Task<ApiResponse> Get() => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, Uri);

    public async virtual Task<ApiResponse> GetById(int id1, int id2) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, $"{Uri}/{id1}/{id2}");

    // POST
    public async virtual Task<ApiResponse> Create(TCreateDto createDto) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Post, Uri, createDto);

    public async virtual Task<ApiResponse> CreateMany(IEnumerable<TCreateDto> createDtos) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Post, Uri, createDtos);

    // DELETE
    public async virtual Task<ApiResponse> Delete(int id1, int id2) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Delete, $"{Uri}/{id1}/{id2}");
}