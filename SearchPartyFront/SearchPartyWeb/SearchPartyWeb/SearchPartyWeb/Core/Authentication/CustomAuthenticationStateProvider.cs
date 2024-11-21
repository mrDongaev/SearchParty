using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;

namespace SearchPartyWeb.Core.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorageService;
    private readonly IAuthService _authService;
    private ClaimsPrincipal anonymus = new ClaimsPrincipal(new ClaimsIdentity());

    public CustomAuthenticationStateProvider(ILocalStorageService localStorageService, IAuthService authService)
    {
        _localStorageService = localStorageService;
        _authService = authService;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var loginResponse = await _localStorageService.GetItemAsStringAsync("Authentication");
            if (String.IsNullOrWhiteSpace(loginResponse))
            {
                return await Task.FromResult(new AuthenticationState(anonymus));

            }

            return await Task.FromResult(
                new AuthenticationState(SetClaims(JsonSerializer.Deserialize<LoginResponse>(loginResponse).DisplayName!)));
        }
        catch (Exception e)
        {
            return await Task.FromResult(new AuthenticationState(anonymus));
            
        }
    }

    public async Task UpdateAuthenticationStateAsync(LoginResponse loginResponse)
    {
        try
        {
            ClaimsPrincipal claimsPrincipal = new();
            if (loginResponse!=null)
            {
                claimsPrincipal = SetClaims(loginResponse.DisplayName!);
                await _localStorageService.SetItemAsStringAsync("Authentication",
                    JsonSerializer.Serialize(loginResponse));
            }
            else
            {
                await _localStorageService.RemoveItemAsync("Authentication");
                claimsPrincipal = anonymus;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
        catch (Exception e)
        {
            await Task.FromResult(new AuthenticationState(anonymus));
        }
    }

    ClaimsPrincipal SetClaims(string email) => new ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { new Claim(ClaimTypes.Name, email) }, "CustomAuth"));
    
}