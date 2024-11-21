namespace SearchPartyWeb.Core.ApiExecutor;

public interface IWebApiExecutor
{
    Task<bool>  InvokeDelete<T>(string uri,string accessToken);
    Task<T> InvokeGet<T>(string uri, string accessToken);
    Task<T> InvokePost<T>(string uri, T obj,string accessToken="");
    Task<T> InvokePost<T, K>(string uri, K obj,string accessToken="");
    Task<bool> InvokePut<T>(string uri, T obj);
   
}