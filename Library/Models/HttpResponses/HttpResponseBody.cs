using FluentResults;

namespace Library.Models.HttpResponses
{
    public class HttpResponseBody
    {
        public bool IsSuccess { get; set; } = true;

        public List<string> Errors { get; set; } = [];

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
            List<string> messages = [];
            List<string> errors = [];
            if (result.IsSuccess)
            {
                messages.AddRange(result.Successes.Select(s => s.Message));
            }
            else if (result.IsFailed)
            {
                errors.AddRange(result.Errors.Select(s => s.Message));
            }
            return new HttpResponseBody
            {
                Errors = errors,
                Messages = messages,
                IsSuccess = result.IsSuccess,
            };
        }

        public static HttpResponseBody MapToHttpResponseBody<TSource>(this Result<TSource> result)
        {
            List<string> messages = [];
            List<string> errors = [];
            if (result.IsSuccess)
            {
                messages.AddRange(result.Successes.Select(s => s.Message));
            }
            else if (result.IsFailed)
            {
                errors.AddRange(result.Errors.Select(s => s.Message));
            }
            return new HttpResponseBody
            {
                Errors = errors,
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
                Errors = messages,
                IsSuccess = result.IsSuccess,
                Data = valueResolver(result),
            };
        }

        public static Result MapToResult(this HttpResponseBody response)
        {
            List<Success> messages = [];
            List<Error> errors = [];
            if (response.IsSuccess)
            {
                messages.AddRange(response.Messages.Select(m => new Success(m)).ToList());
            }
            else
            {
                errors.AddRange(response.Errors.Select(e => new Error(e)).ToList());
            }
            return new Result()
                .WithErrors(errors)
                .WithSuccesses(messages);
        }

        public static Result<TData?> MapToResult<TData>(this HttpResponseBody<TData?> response)
        {
            List<Success> messages = [];
            List<Error> errors = [];
            if (response.IsSuccess)
            {
                messages.AddRange(response.Messages.Select(m => new Success(m)).ToList());
            }
            else
            {
                errors.AddRange(response.Errors.Select(e => new Error(e)).ToList());
            }
            return new Result<TData?>()
                .WithErrors(errors)
                .WithSuccesses(messages)
                .WithValue(response.Data);
        }
    }

}
