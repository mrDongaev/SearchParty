using SearchPartyWeb.Core.Models;

namespace SearchPartyWeb.Core.ProfileRepository;

public interface IProfileService
{
    Task<List<Profile>> GetUserProfilesAsync(string id,string accessToken);
    Task<List<Team>> GetTeamsListAsync(string accessToken);
    Task<List<Team>> GetUserTeamsAsync(string id, string accessToken);
    Task<Profile> AddNewUserProfileAsync(Profile profile, string accessToken);
    Task<List<Position>> GetPositionsAsync(string accessToken);
    Task<List<Hero>> GetHerosAsync(string accessToken);
    Task<CreateTeamModel> AddNewTeamAsync(CreateTeamModel team,string accessToken);
    Task<bool> DeleteProfileAsync(string id, string accessToken);
    Task<bool> DeleteTeamAsync(string id, string accessToken);
}