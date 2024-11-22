using SearchPartyWeb.Core.ApiExecutor;
using SearchPartyWeb.Core.Models;

namespace SearchPartyWeb.Core.InvitationRepository;

public class InvitationService : IInvitationService
{
    public IWebApiExecutor _webApiExecutor;

    public InvitationService(WebApiExecutor webApiExecutor)
    {
        _webApiExecutor = webApiExecutor;
    }

    public async Task<List<Invantion>> GetMyInvantionsAsync(string[] states, string accessToken)
    {
        var response =
            await _webApiExecutor.InvokePost<List<Invantion>,string[]>($"api/PlayerInvitation/GetUserMessages",states, accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }
    
    public async Task<List<Invantion>> GetMyRequestsAsync(string[] states, string accessToken)
    {
        var response =
            await _webApiExecutor.InvokePost<List<Invantion>,string[]>($"api/TeamApplication/GetUserMessages",states, accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }
}