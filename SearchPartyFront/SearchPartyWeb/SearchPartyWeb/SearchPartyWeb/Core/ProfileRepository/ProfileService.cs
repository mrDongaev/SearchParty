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

    public async Task<List<Profile>> GetUserProfilesAsync(string id, string accessToken)
    {
        var response =
            await _webApiExecutor.InvokeGet<List<Profile>>($"api/Player/GetPlayersByUserId/{id}", accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }

    public async Task<List<Team>> GetUserTeamsAsync(string id, string accessToken)
    {
        var response = await _webApiExecutor.InvokeGet<List<Team>>($"api/Team/GetTeamsByUserId/{id}", accessToken);
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
    
    public async Task<Profile> AddNewUserProfileAsync(Profile profile,string accessToken)
    {
        var response = await _webApiExecutor.InvokePost<Profile>($"api/Player/Create",profile ,accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }

    public async Task<List<Profile>> GetUserProfilesAsync(string id, string accessToken)
    {
        var response =
            await _webApiExecutor.InvokeGet<List<Profile>>($"api/Player/GetPlayersByUserId/{id}", accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }

    public async Task<List<Team>> GetUserTeamsAsync(string id, string accessToken)
    {
        var response = await _webApiExecutor.InvokeGet<List<Team>>($"api/Team/GetTeamsByUserId/{id}", accessToken);
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
    
    public async Task<Profile> AddNewUserProfileAsync(Profile profile,string accessToken)
    {
        var response = await _webApiExecutor.InvokePost<Profile>($"api/Player/Create",profile ,accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }

    public async Task<List<Profile>> GetUserProfilesAsync(string id, string accessToken)
    {
        var response =
            await _webApiExecutor.InvokeGet<List<Profile>>($"api/Player/GetPlayersByUserId/{id}", accessToken);
        if (response != null)
        {
            return response;
        }

        return null;
    }

    public async Task<List<Team>> GetUserTeamsAsync(string id, string accessToken)
    {
        var response = await _webApiExecutor.InvokeGet<List<Team>>($"api/Team/GetTeamsByUserId/{id}", accessToken);
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