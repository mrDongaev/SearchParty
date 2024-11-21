using SearchPartyWeb.Core.ApiExecutor;

namespace SearchPartyWeb.Core.ProfileRepository;


public class ProfileService : IProfileService
{
    public IWebApiExecutor _webApiExecutor;

    public ProfileService(WebApiExecutor webApiExecutor)
    {
        _webApiExecutor = webApiExecutor;
    }
}