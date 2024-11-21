namespace SearchPartyWeb.Core.ApiExecutor;

public interface IWebApiExecutor
{
    Task InvokeDelete<T>(string uri);
    Task<T> InvokeGet<T>(string uri);
    Task<T> InvokePost<T>(string uri, T obj);
    Task<bool> InvokePut<T>(string uri, T obj);
   
}