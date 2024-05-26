using LibraryManagementSystem.WinUI.ApiConnection;
using LibraryManagementSystem.WinUI.ApiConnection.Enums;

namespace LibraryManagementSystem.WinUI.Service.Base;

public abstract class CreateUpdateManyRequestService<TCreateDto, TUpdateDto> where TCreateDto : class, new() where TUpdateDto : class, new()
{
    private readonly AppSettings _settings = AppSettings.Instance;
    protected readonly string Uri;

    public CreateUpdateManyRequestService(string? endpointUrl)
    {
        Uri = $"{_settings.UrlApi}{endpointUrl}";
    }

    // GET
    public async virtual Task<ApiResponse> Get() => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, Uri);

    public async virtual Task<ApiResponse> GetById(int id) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Get, $"{Uri}/{id}");

    // POST
    public async virtual Task<ApiResponse> Create(TCreateDto createDto) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Post, Uri, createDto);

    public async virtual Task<ApiResponse> CreateMany(IEnumerable<TCreateDto> createDtos) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Post, Uri, createDtos);

    // PUT
    public async virtual Task<ApiResponse> Update(TUpdateDto updateDto) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Put, Uri, updateDto);

    public async virtual Task<ApiResponse> UpdateMany(IEnumerable<TUpdateDto> updateDtos) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Put, Uri, updateDtos);

    // DELETE
    public async virtual Task<ApiResponse> Delete(int id) => await HttpRequest.RequestAsync<ApiResponse>(HttpRequestMethod.Delete, $"{Uri}/{id}");
}