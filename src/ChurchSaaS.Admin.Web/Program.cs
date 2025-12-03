using MudBlazor.Services;
using ChurchSaaS.Admin.Web.Components;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ChurchSaaS.Admin.Infrastructure;
using ChurchSaaS.Admin.Infrastructure.Persistence;
using MediatR;
using FluentValidation;
using ChurchSaaS.Admin.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<AdminDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AdminDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddMediatR(typeof(ChurchSaaS.Admin.Application.Commands.ChurchClients.CreateChurchClientCommand).Assembly);
builder.Services.AddValidatorsFromAssemblyContaining<CreatePlanCommandValidator>();
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

app.MapPost("/auth/login", async (LoginRequest request, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager) =>
{
    var user = await userManager.FindByEmailAsync(request.Email);
    if (user == null)
        return Results.Unauthorized();

    var result = await signInManager.PasswordSignInAsync(user.UserName!, request.Password, true, lockoutOnFailure: false);
    if (!result.Succeeded)
        return Results.Unauthorized();

    return Results.Ok();
});

app.MapPost("/auth/logout", async (SignInManager<IdentityUser> signInManager) =>
{
    await signInManager.SignOutAsync();
    return Results.Ok();
});

// Seed admin user/role
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    const string adminRole = "Admin";
    const string adminEmail = "leoterio@gmail.com";
    const string adminPassword = "Master@123";

    if (!await roleManager.RoleExistsAsync(adminRole))
    {
        await roleManager.CreateAsync(new IdentityRole(adminRole));
    }

    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        adminUser = new IdentityUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };
        var createResult = await userManager.CreateAsync(adminUser, adminPassword);
        if (createResult.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(adminUser, adminRole))
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}

app.Run();

public record LoginRequest(string Email, string Password);
