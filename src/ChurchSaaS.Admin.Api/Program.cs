using ChurchSaaS.Admin.Application;
using ChurchSaaS.Admin.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "ChurchSaaS.Admin API v1");
    });
}

app.MapControllers();
app.MapGet("/", () => Results.Ok("ChurchSaaS.Admin API is running"));

app.Run();
