namespace Library.Results.Errors.Http
{
    public class HttpRequestFailedError : ErrorWithData
    {
        public HttpRequestFailedError(object? data = null) : this("Http request has failed", data) { }

        public HttpRequestFailedError(string message, object? data = null) : base(message, data)
        {
            Metadata["ReasonName"] = nameof(HttpRequestFailedError);
        }
    }
}
