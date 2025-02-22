using FluentResults;
using Library.Models.HttpResponses;

namespace Library.Utils
{
    public static class HttpClientUtils
    {
        public static async Task<Result<T?>> ReadResultFromJsonResponse<T>(this HttpResponseMessage response, string? exceptionMessage = default, CancellationToken cancellationToken = default)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException(exceptionMessage);
            }

            var body = await response.Content.ReadFromJsonAsync<HttpResponseBody<T>>(cancellationToken);
            if (body != null)
            {
                return body.MapToResult();
            }
            else
            {
                throw new HttpRequestException(exceptionMessage);
            }
        }
    }
}
