using SearchPartyWeb.Core.Models;

namespace SearchPartyWeb.Core.InvitationRepository;

public interface IInvitationService
{
    Task<List<Invantion>> GetMyInvantionsAsync(string[] states, string accessToken);
    Task<List<Invantion>> GetMyRequestsAsync(string[] states, string accessToken);

}