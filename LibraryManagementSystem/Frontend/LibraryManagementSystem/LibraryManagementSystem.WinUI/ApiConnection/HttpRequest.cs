using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using LibraryManagementSystem.WinUI.ApiConnection.Enums;

namespace LibraryManagementSystem.WinUI.ApiConnection;

public static class HttpRequest
{
    public static async Task<TResult> RequestAsync<TResult>(HttpRequestMethod method, string uri, object? data = null)
    {
        HttpClient client = new()
        {
            BaseAddress = new Uri(uri)
        };
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = method switch
        {
            HttpRequestMethod.Get => await client.GetAsync(uri),
            HttpRequestMethod.Post => await client.PostAsJsonAsync(uri, data),
            HttpRequestMethod.Put => await client.PutAsJsonAsync(uri, data),
            HttpRequestMethod.Delete => await client.DeleteAsync(uri),
            _ => null
        };

        if (response is not null && response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResult>(content);
            return result is null ? throw new InvalidOperationException("La respuesta no se pudo deserializar") : result;
        }
        else if (response is not null && !response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"{response.StatusCode.GetHashCode()} - {response.StatusCode}\nContent: {content}");
        }
        throw new Exception(uri);
    }
}
