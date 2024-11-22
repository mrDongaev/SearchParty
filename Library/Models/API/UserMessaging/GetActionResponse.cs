﻿using Library.Models.Enums;

namespace Library.Models.API.UserMessaging
{
    public static class GetActionResponse
    {
        public sealed class Response<T> where T : GetMessage.Response
        {
            public ActionResponseStatus Status { get; set; }

            public string ActionMessage { get; set; } = string.Empty;

            public T? Message { get; set; }
        }
    }
}
