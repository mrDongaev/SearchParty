using FluentResults;
using Library.Results.Utils;

namespace Library.Models.HttpResponses
{
    public class HttpResponseBody
    {
        public readonly string Type = "HttpResponseBody";

        public bool IsSuccess { get; set; } = true;

        public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();

        public IDictionary<string, string[]> Messages { get; set; } = new Dictionary<string, string[]>();

        public Result MapToResult()
        {
            List<Success> messages = [];
            List<Error> errors = [];
            if (IsSuccess)
            {
                foreach (var message in Messages)
                {
                    foreach (var text in message.Value)
                    {
                        messages.Add(ResultUtils.GetSuccessByKey(message.Key, text));
                    }
                }
            }
            else
            {
                foreach (var message in Errors)
                {
                    foreach (var text in message.Value)
                    {
                        errors.Add(ResultUtils.GetErrorByKey(message.Key, text));
                    }
                }
            }
            return new Result()
                .WithErrors(errors)
                .WithSuccesses(messages);
        }
    }

    public class HttpResponseBody<T> : HttpResponseBody
    {
        public T? Data { get; set; }

        public new Result<T?> MapToResult()
        {
            List<Success> messages = [];
            List<Error> errors = [];
            if (IsSuccess)
            {
                foreach (var message in Messages)
                {
                    foreach (var text in message.Value)
                    {
                        messages.Add(ResultUtils.GetSuccessByKey(message.Key, text));
                    }
                }
            }
            else
            {
                foreach (var message in Errors)
                {
                    foreach (var text in message.Value)
                    {
                        errors.Add(ResultUtils.GetErrorByKey(message.Key, text));
                    }
                }
            }
            var result = new Result<T?>()
                .WithErrors(errors)
                .WithSuccesses(messages);

            if (result.IsSuccess)
            {
                result.WithValue(Data);
            }

            return result;
        }
    }

    public static class HttpResponseBodyMapper
    {
        private static IDictionary<string, string[]> MapReasonsToMessages(IEnumerable<IReason> reasons, string defaultKey)
        {
            Dictionary<string, List<string>> messages = [];
            foreach (var reason in reasons)
            {
                if (!reason.Metadata.TryGetValue("ReasonName", out object? reasonName))
                {
                    reasonName = defaultKey;
                }
                if (!messages.TryGetValue((string)reasonName, out var strings))
                {
                    strings = [];
                    messages.Add((string)reasonName, strings);
                }
                strings.Add(reason.Message);
            }
            return messages.Select(d => KeyValuePair.Create(d.Key, d.Value.ToArray())).ToDictionary();
        }

        public static HttpResponseBody MapToHttpResponseBody(this Result result)
        {
            var body = new HttpResponseBody()
            {
                IsSuccess = result.IsSuccess,
            };
            if (result.IsSuccess)
            {
                body.Messages = MapReasonsToMessages(result.Successes, "Success");
            }
            else if (result.IsFailed)
            {
                body.Errors = MapReasonsToMessages(result.Errors, "Error");
            }
            return body;
        }

        public static HttpResponseBody MapToHttpResponseBody<TSource>(this Result<TSource> result)
        {
            return MapToHttpResponseBody(result);
        }

        public static HttpResponseBody<TDestination> MapToHttpResponseBody<TSource, TDestination>(this Result<TSource> result, Func<Result<TSource>, TDestination> valueResolver)
        {
            TDestination? data = default;
            try
            {
                data = valueResolver(result);
            }
            catch { }

            var body = new HttpResponseBody<TDestination>()
            {
                IsSuccess = result.IsSuccess,
                Data = data,
            };
            if (result.IsSuccess)
            {
                body.Messages = MapReasonsToMessages(result.Successes, "Success");
            }
            else if (result.IsFailed)
            {
                body.Errors = MapReasonsToMessages(result.Errors, "Error");
            }
            return body;
        }
    }

}
