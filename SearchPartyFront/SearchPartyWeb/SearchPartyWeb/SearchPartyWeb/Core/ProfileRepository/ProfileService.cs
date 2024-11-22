using System.Text.Json;
using SearchPartyWeb.Core.ApiExecutor;
using SearchPartyWeb.Core.Models;

namespace SearchPartyWeb.Core.ProfileRepository;

public class ProfileService : IProfileService
{
    public IWebApiExecutor _webApiExecutor;

    public ProfileService(WebApiExecutor webApiExecutor)
    {
        _webApiExecutor = webApiExecutor;
    }

    public async Task<List<Team>> GetUserTeamsAsync(string id, string accessToken)
    {
        var response = await _webApiExecutor.InvokeGet<List<Team>>($"api/Team/GetTeamsOfUser", accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }


    public async Task<List<Team>> GetTeamsListAsync(string accessToken)
    {
        var response = await _webApiExecutor.InvokeGet<List<Team>>($"api/Team/GetAll", accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }
    
    public async Task<List<Profile>> GetProfilesListAsync(string accessToken)
    {
        var response = await _webApiExecutor.InvokeGet<List<Profile>>($"api/Player/GetAll", accessToken);
        if (response != null)
        {
            return response;
        }
        return null;
    }
    
    public async Task AddPlayerToTeamAsync(string playerId,string teamId,long position,string accessToken)
    {
        await _webApiExecutor.InvokePost<string>($"api/PlayerBoard/InvitePlayerToTeam/{playerId}/{teamId}/{position}",null, accessToken);
    }
    
    public async Task AddTeamToPlayerAsync(string teamId,string playerId,long position,string accessToken)
    {
        await _webApiExecutor.InvokePost<string>($"api/TeamBoard/SendTeamApplicationRequest/{teamId}/{playerId}/{position}",null, accessToken);
           
    }
    
   
    public async Task<List<Profile>> GetUserProfilesAsync(string id, string accessToken)
    {
        var response =
            await _webApiExecutor.InvokeGet<List<Profile>>($"api/Player/GetPlayersOfUser", accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }

   
    public async Task<Profile> AddNewUserProfileAsync(Profile profile,string accessToken)
    {
        var response = await _webApiExecutor.InvokePost<Profile>($"api/Player/Create",profile ,accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }
    
    public async Task<CreateTeamModel> AddNewTeamAsync(CreateTeamModel team,string accessToken)
    {
        var response = await _webApiExecutor.InvokePost<CreateTeamModel>($"api/Team/Create",team,accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }
    public async Task<bool> DeleteProfileAsync(string id,string accessToken)
    {
        var response = await _webApiExecutor.InvokeDelete<bool>($"api/Player/Delete/{id}",accessToken);
        if (response == true)
        {
            return response;
        }

        return false;
    }
    public async Task<bool> DeleteTeamAsync(string id,string accessToken)
    {
        var response = await _webApiExecutor.InvokeDelete<bool>($"api/Team/Delete/{id}",accessToken);
        if (response == true)
        {
            return response;
        }
        return false;
    }
    
    
    public async Task<List<Hero>> GetHerosAsync(string accessToken)
    {
        var response = await _webApiExecutor.InvokeGet<List<Hero>>($"api/Hero/GetAll", accessToken);
        if (response != null)
        {
            return response;
        }
        return null;
    }
    public async Task<List<Position>> GetPositionsAsync(string accessToken)
    {
        var response = await _webApiExecutor.InvokeGet<List<Position>>($"api/Position/GetAll", accessToken);
        if (response != null)
        {
            return response;
        }
        return null;
    }
    
}