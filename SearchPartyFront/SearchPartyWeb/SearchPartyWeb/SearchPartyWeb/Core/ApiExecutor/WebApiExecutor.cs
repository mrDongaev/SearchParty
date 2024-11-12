using System.Net.Http.Headers;
using System.Net.Mime;

namespace SearchPartyWeb.Core.ApiExecutor;

public class WebApiExecutor : IWebApiExecutor
{
    private readonly string _urlBase;
    private readonly HttpClient _httpClient;

    public WebApiExecutor(string urlBase, HttpClient httpClient)
    {
        _urlBase = urlBase;
        _httpClient = httpClient;

        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
       _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task InvokeDelete<T>(string uri)
    {
        var response = await _httpClient.DeleteAsync(GetUrl(uri));
        await HandleError(response);
    }

    public async Task<T> InvokeGet<T>(string uri)
    {
        var response = await _httpClient.GetFromJsonAsync<T>(GetUrl(uri));
        return response;
    }

    public async Task<T> InvokePost<T>(string uri, T obj)
    {
      
        var response = await _httpClient.PostAsJsonAsync(GetUrl(uri), obj);
        return await response.Content.ReadFromJsonAsync<T>();
    }

      

    public async Task<bool> InvokePut<T>(string uri, T obj)
    {
        var response = await _httpClient.PutAsJsonAsync<T>(GetUrl(uri), obj);
        await HandleError(response);           
        return await response.Content.ReadFromJsonAsync<bool>();
           
    }

    public string GetUrl(string uri) => $"{_urlBase}{uri}";

    private static async Task HandleError(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(error);
        }
    }
}
