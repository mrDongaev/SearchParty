﻿namespace Application.User
{
    public class AuthenticatedUser
    {
        public string? Email { get; set; }

        public string? DisplayName { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public string? Id { get; set; }
    }
}
