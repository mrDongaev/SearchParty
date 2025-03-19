using Application.User;
using Domain;

namespace Application.Interfaces
{
    public interface IRefreshGenerator
    {
        UserData RefreshToken(AppUser appUser, UserData userData);

        string DecodeRefreshToken(string refreshToken, string parameter);
    }
}
