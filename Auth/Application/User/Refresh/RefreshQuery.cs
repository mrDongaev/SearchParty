using MediatR;

namespace Application.User.Refresh
{
    public class RefreshQuery : IRequest<UserData>
    {
        public string? RefreshToken { get; set; }
    }
}
