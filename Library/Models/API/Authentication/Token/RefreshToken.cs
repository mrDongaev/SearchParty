namespace Library.Models.API.Authentication.Token
{
    public static class RefreshToken
    {
        public sealed class Request
        {
            public string RefreshToken { get; set; }
        }

        public sealed class Response
        {
            public string AccessToken { get; set; }

            public string RefreshToken { get; set; }
        }
    }
}
