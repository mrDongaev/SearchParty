using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

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
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

    }

    public async Task<bool> InvokeDelete<T>(string uri,string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",accessToken);
        var response = await _httpClient.DeleteAsync(GetUrl(uri));
        await HandleError(response);
        var result = JsonSerializer.Serialize(response);
        if (response.StatusCode.ToString()=="OK")
        {
            return true;
        }
        return false;
    }

    public async Task<T> InvokeGet<T>(string uri,string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",accessToken);
        var response = await _httpClient.GetFromJsonAsync<T>(GetUrl(uri));
        return response;
    }

    public async Task<T> InvokePost<T>(string uri, T obj, string accessToken="") 
    {
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",accessToken);
        var d = JsonSerializer.Serialize(obj);
        var response = await _httpClient.PostAsJsonAsync(GetUrl(uri), obj);
        await HandleError(response);
        return await response.Content.ReadFromJsonAsync<T>();
    }

    

    public async Task<T> InvokePost<T, K>(string uri, K obj,string accessToken="")
    {
        var d = JsonSerializer.Serialize(obj);
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",accessToken);
        var response = await _httpClient.PostAsJsonAsync(GetUrl(uri), obj);
        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<bool> InvokePut<T>(string uri, T obj)
    {
        var response = await _httpClient.PutAsJsonAsync<T>(GetUrl(uri), obj);
        await HandleError(response);
        return await response.Content.ReadFromJsonAsync<bool>();
    }

    string GetUrl(string uri) => $"{_urlBase}{uri}";

    private static async Task HandleError(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(error);
        }
    }
}