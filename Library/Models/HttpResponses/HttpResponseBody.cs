using FluentResults;

namespace Library.Models.HttpResponses
{
    public class HttpResponseBody
    {
        public bool IsSuccess { get; set; } = true;

        public List<string> Messages { get; set; } = [];
    }

    public class HttpResponseBody<T> : HttpResponseBody
    {
        public T? Data { get; set; }
    }

    public static class HttpResponseBodyMapper
    {
        public static HttpResponseBody MapToHttpResponseBody(this Result result)
        {
            List<string> messages = new List<string>();
            if (result.IsSuccess)
            {
                messages.AddRange(result.Successes.Select(s => s.Message));
            }
            else if (result.IsFailed)
            {
                messages.AddRange(result.Errors.Select(s => s.Message));
            }
            return new HttpResponseBody
            {
                Messages = messages,
                IsSuccess = result.IsSuccess,
            };
        }

        public static HttpResponseBody MapToHttpResponseBody<TSource>(this Result<TSource> result)
        {
            List<string> messages = new List<string>();
            if (result.IsSuccess)
            {
                messages.AddRange(result.Successes.Select(s => s.Message));
            }
            else if (result.IsFailed)
            {
                messages.AddRange(result.Errors.Select(s => s.Message));
            }
            return new HttpResponseBody
            {
                Messages = messages,
                IsSuccess = result.IsSuccess,
            };
        }

        public static HttpResponseBody<TDestination> MapToHttpResponseBody<TSource, TDestination>(this Result<TSource> result, Func<Result<TSource>, TDestination> valueResolver)
        {
            List<string> messages = new List<string>();
            if (result.IsSuccess)
            {
                messages.AddRange(result.Successes.Select(s => s.Message));
            }
            else if (result.IsFailed)
            {
                messages.AddRange(result.Errors.Select(s => s.Message));
            }
            return new HttpResponseBody<TDestination>
            {
                Messages = messages,
                IsSuccess = result.IsSuccess,
                Data = valueResolver(result),
            };
        }
    }

}
