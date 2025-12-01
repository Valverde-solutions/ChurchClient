using MudBlazor.Services;
using ChurchSaaS.Admin.Web.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
builder.Services.AddHttpClient();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.Cookie.Name = "auth";
    });

builder.Services.AddSingleton<IPasswordHasher<IdentityUser>, PasswordHasher<IdentityUser>>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/auth/login", async (LoginRequest request, IPasswordHasher<IdentityUser> hasher, HttpContext http) =>
{
    // TODO: replace with real user store (Identity DB)
    const string masterEmail = "leoterio@gmail.com";
    const string storedHash = "AQAAAAIAAYagAAAAEO2H3J+q+MCE5SZftP8edeke3bgVkjjjJAdpFu8Adh5vnyp/mGW6u033YUR6OH8eRg==";

    if (!string.Equals(request.Email, masterEmail, StringComparison.OrdinalIgnoreCase))
    {
        return Results.Unauthorized();
    }

    var user = new IdentityUser { Id = "master-user", UserName = masterEmail, Email = masterEmail, NormalizedUserName = masterEmail.ToUpperInvariant(), NormalizedEmail = masterEmail.ToUpperInvariant() };
    var result = hasher.VerifyHashedPassword(user, storedHash, request.Password);
    if (result is PasswordVerificationResult.Failed)
    {
        return Results.Unauthorized();
    }

    var claims = new[]
    {
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id),
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName ?? masterEmail),
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, user.Email ?? masterEmail),
        new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Admin")
    };
    var identity = new System.Security.Claims.ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new System.Security.Claims.ClaimsPrincipal(identity);

    await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    return Results.Ok();
});

app.MapPost("/auth/logout", async (HttpContext http) =>
{
    await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Ok();
});

app.Run();

public record LoginRequest(string Email, string Password);
