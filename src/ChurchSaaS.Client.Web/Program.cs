using MudBlazor.Services;
using ChurchSaaS.Client.Web.Components;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChurchSaaS.Admin.Infrastructure;
using ChurchSaaS.Admin.Infrastructure.Persistence;
using ChurchSaaS.Client.Infrastructure.Identity;
using MediatR;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<AdminDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AdminDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMediatR(typeof(ChurchSaaS.Client.Application.Commands.ChurchClients.CreateChurchClientCommand).Assembly);

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.Cookie.Name = "auth";
    });

builder.Services.AddInfrastructure();

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

app.MapPost("/auth/login", async (LoginRequest request, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager) =>
{
    var user = await userManager.FindByEmailAsync(request.Email);
    if (user == null)
        return Results.Unauthorized();

    var result = await signInManager.PasswordSignInAsync(user.UserName!, request.Password, true, lockoutOnFailure: false);
    if (!result.Succeeded)
        return Results.Unauthorized();

    return Results.Ok();
});

app.MapPost("/auth/logout", async (SignInManager<ApplicationUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
});

// Seed platform roles/users
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var roles = new[]
    {
        "PlatformAdmin",
        "PlatformSupport",
        "ChurchAdmin",
        "Pastor",
        "Leader",
        "Member"
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }
    }

    const string adminEmail = "leoterio@gmail.com";
    const string adminPassword = "Master@123";

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            DisplayName = "Platform Admin",
            IsPlatformAdmin = true
        };
        var createResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "PlatformAdmin");
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(adminUser, "PlatformAdmin"))
        {
            await userManager.AddToRoleAsync(adminUser, "PlatformAdmin");
        }

        if (!adminUser.IsPlatformAdmin)
        {
            adminUser.IsPlatformAdmin = true;
            await userManager.UpdateAsync(adminUser);
        }
    }
}

app.Run();

public record LoginRequest(string Email, string Password);
