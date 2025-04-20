using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using RahmanMemberVault.Api.Extensions;
using RahmanMemberVault.Infrastructure.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using Serilog;


// Configure Serilog for logging
Directory.CreateDirectory("ExceptionLogs");

// Set up Serilog to log to console and file
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("ExceptionLogs/logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Logger.Information("Starting RahmanMemberVault API...");


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Application & Infrastructure
builder.Services
       .AddApplicationLayer()
       .AddInfrastructureLayer(builder.Configuration);

// Add AppExceptionHandler
builder.Services.AddExceptionHandler<AppExceptionHandler>();

// Configure the exception handler options
builder.Services.Configure<ExceptionHandlerOptions>(options =>
{
    options.AllowStatusCode404Response = true;
});
builder.Services.AddProblemDetails();

// register the SwaggerGen
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "RahmanMemberVault API",
        Version = "v1"
    });
});


builder.Host.UseSerilog(); // Use Serilog for logging
var app = builder.Build();

// ensure App_Data exists on disk
var dataDir = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
if (!Directory.Exists(dataDir))
{
    Directory.CreateDirectory(dataDir);
}

// Deploy the database in app statrup from Migration
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
    // Optionally serve at root
    // c.RoutePrefix = string.Empty;
});

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
