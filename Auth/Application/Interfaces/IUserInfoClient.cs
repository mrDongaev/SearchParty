using Application.User.ExternalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserInfoClient
    {
        Task<HttpResponseMessage> CreateUserInfoAsync(CreateUserInfoRequest createUserInfoRequest, string accessToken);
    }
}
