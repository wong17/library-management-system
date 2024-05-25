using LibraryManagementSystem.Desktop.ApiConnection.Enums;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace LibraryManagementSystem.Desktop.ApiConnection
{
    public static class HttpRequest
    {
        public static async Task<TResult> RequestAsync<TResult>(HttpRequestMethod method, string uri, object? data = null)
        {
            HttpClient client = new()
            {
                BaseAddress = new Uri(uri)
            };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage? response = method switch
            {
                HttpRequestMethod.Get => await client.GetAsync(uri),
                HttpRequestMethod.Post => await client.PostAsJsonAsync(uri, data),
                HttpRequestMethod.Put => await client.PutAsJsonAsync(uri, data),
                HttpRequestMethod.Delete => await client.DeleteAsync(uri),
                _ => null
            };

            if (response is not null && response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<TResult>(content);
                return result is null ? throw new InvalidOperationException("Los datos no se pudieron deserializar correctamente.") : result;
            }
            else if (response is not null && !response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                throw new Exception($"{response.StatusCode.GetHashCode()} - {response.StatusCode}\nContent: {content}");
            }
            throw new Exception(uri);
        }
    }
}
