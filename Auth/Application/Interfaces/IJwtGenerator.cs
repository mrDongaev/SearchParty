using Application.User.Settings;
using Domain;

namespace Application.Interfaces
{
    public interface IJwtGenerator
    {
        UserToken CreateJwtToken(AppUser user);
    }
}
