
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using SearchPartyWeb.Components;
using SearchPartyWeb.Core.ApiExecutor;
using SearchPartyWeb.Core.Authentication;
using SearchPartyWeb.Core.ProfileRepository;
using SearchPartyWeb.Core.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddMudServices();
builder.Services.AddSingleton<IAuthService>(
    new AuthService(
        new WebApiExecutor("http://localhost:8084/", new HttpClient())
    ) 
);
builder.Services.AddSingleton<IProfileService>(
    new ProfileService(
        new WebApiExecutor("http://localhost:8080/", new HttpClient())
    ) 
);

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();