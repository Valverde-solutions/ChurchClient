var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ChurchSaaS.Admin API v1");
    });
}

app.MapControllers();
app.MapGet("/", () => Results.Ok("ChurchSaaS.Admin API is running"));

app.Run();
