using System.Reflection;
using ChurchSaaS.Client.Api.Endpoints;
using ChurchSaaS.Client.Application.Commands.ChurchClients;
using ChurchSaaS.Admin.Infrastructure;
using ChurchSaaS.Admin.Infrastructure.Persistence;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ChurchSaaS.Admin API",
        Version = "v1",
        Description = "Public endpoints for ChurchSaaS Admin"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }
});

builder.Services.AddDbContext<AdminDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddMediatR(typeof(CreateChurchClientCommand).Assembly);


builder.Services.AddInfrastructure();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ChurchSaaS.Admin API v1");
});

app.MapControllers();

app.MapGet("/", () => Results.Ok("ChurchSaaS.Admin API is running"));

app.Run();
