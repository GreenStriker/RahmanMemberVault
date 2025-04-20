using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using RahmanMemberVault.Api.Extensions;
using RahmanMemberVault.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Configure Serilog from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

//Add framework services
builder.Services.AddControllers();

//Register Clean‑Architecture layers
builder.Services
    .AddApplicationLayer()
    .AddInfrastructureLayer(builder.Configuration, builder.Environment);

//Global exception handling
builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.Configure<ExceptionHandlerOptions>(
    builder.Configuration.GetSection("ExceptionHandlerOptions"));
builder.Services.AddProblemDetails();

//Swagger/OpenAPI (enabled in all environments)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "RahmanMemberVault API",
        Version = "v1"
    });
});

var app = builder.Build();

//Apply pending EF Core migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}


// Enable Swagger UI in all environments
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RahmanMemberVault API V1");
    c.RoutePrefix = string.Empty;// Serve at root
});

// global exception handler
app.UseExceptionHandler();

// standard middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();