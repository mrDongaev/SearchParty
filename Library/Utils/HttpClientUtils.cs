using FluentResults;
using Library.Models.HttpResponses;
using System.Net;
using System.Text.Json;

namespace Library.Utils
{
    public static class HttpClientUtils
    {
        public static async Task<Result<T?>> ReadResultFromJsonResponse<T>(this HttpResponseMessage response, string? exceptionMessage = default, CancellationToken cancellationToken = default)
        {
            string json = await response.Content.ReadAsStringAsync(cancellationToken);
            try
            {
                if (json != null)
                {
                    JsonElement jsonElement = JsonDocument.Parse(json).RootElement;
                    if (jsonElement.ValueKind == JsonValueKind.Object && jsonElement.TryGetProperty("Type", out var type) && type.ValueEquals("HttpResponseBody"))
                    {
                        HttpResponseBody<T?>? body = JsonSerializer.Deserialize<HttpResponseBody<T?>>(jsonElement);

                        if (body == null)
                        {
                            throw new HttpRequestException($"{exceptionMessage}: {json}");
                        }

                        return body.MapToResult();
                    }
                    else
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var data = JsonSerializer.Deserialize<T?>(jsonElement);
                            return Result.Ok(data);
                        }
                        else
                        {
                            return Result.Fail($"{exceptionMessage}: {json}");
                        }

                    }

                    throw new InvalidOperationException("Unknown JSON structure");
                }
            }
            catch
            {
                throw new HttpRequestException($"{exceptionMessage}: {json}");
            }

            throw new HttpRequestException($"{exceptionMessage}: {json}");
        }
    }
}
