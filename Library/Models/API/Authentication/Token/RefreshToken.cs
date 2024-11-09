namespace Library.Models.API.Authentication.Token
{
    public static class RefreshToken
    {
        public sealed class Request
        {
            public string refreshToken { get; set; }
        }

        public sealed class Response
        {
            public string accessToken { get; set; }

            public string refreshToken { get; set; }
        }
    }
}
