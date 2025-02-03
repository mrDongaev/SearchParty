using FluentResults;

namespace Library.Results.Errors.Http
{
    public class HttpRequestFailedError : Error
    {
        public HttpRequestFailedError() : this("Http request has failed") { }

        public HttpRequestFailedError(string message) : base(message)
        {
            WithMetadata("key", nameof(HttpRequestFailedError));
        }
    }
}
