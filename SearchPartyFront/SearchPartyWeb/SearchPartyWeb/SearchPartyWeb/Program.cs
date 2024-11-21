
using Microsoft.AspNetCore.Authentication.Cookies;
using MudBlazor.Services;
using SearchPartyWeb.Components;
using SearchPartyWeb.Core.ApiExecutor;
using SearchPartyWeb.Core.Authentication;
using SearchPartyWeb.Core.ProfileRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

//builder.Services.AddSingleton<IWebApiExecutor, WebApiExecutor>();
builder.Services.AddMudServices();
builder.Services.AddSingleton<IProfileService>(
    new ProfileService(
        new WebApiExecutor(Environment.GetEnvironmentVariable("TEAM_PLAYER_PROFILES_URL"), new HttpClient())
        ) 
    );
builder.Services.AddSingleton<IAuthService>(
    new AuthService(
        new WebApiExecutor("http://localhost:8084/", new HttpClient())
    ) 
);

// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.Cookie.Name = "auth_token";
//         options.LoginPath = "/login";
//         options.Cookie.MaxAge = TimeSpan.FromDays(10);
//         options.AccessDeniedPath = "/access-denied";
//     });
// builder.Services.AddAuthorization();
// builder.Services.AddCascadingAuthenticationState();

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
/*app.UseAuthentication();
app.UseAuthorization();*/

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();