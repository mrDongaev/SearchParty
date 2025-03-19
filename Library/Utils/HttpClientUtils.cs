using FluentResults;
using Library.Models.HttpResponses;
using System.Text.Json;

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

            string json = await response.Content.ReadAsStringAsync(cancellationToken);
            if (json != null)
            {
                JsonElement jsonObject = JsonDocument.Parse(json).RootElement;
                if (jsonObject.TryGetProperty("Type", out var type) && type.ValueEquals("HttpResponseBody"))
                {
                    HttpResponseBody<T?>? body = JsonSerializer.Deserialize<HttpResponseBody<T?>>(jsonObject);

                    if (body == null)
                    {
                        throw new HttpRequestException(exceptionMessage);
                    }

                    return body.MapToResult();
                }
                else
                {
                    var data = JsonSerializer.Deserialize<T>(jsonObject);
                    return Result.Ok(data);
                }

                throw new InvalidOperationException("Unknown JSON structure");
            }

            throw new HttpRequestException(exceptionMessage);
        }
    }
}
