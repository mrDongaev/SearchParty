using Application.User;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRefreshGenerator
    {
        UserData RefreshToken(AppUser appUser, UserData userData);
    }
}
