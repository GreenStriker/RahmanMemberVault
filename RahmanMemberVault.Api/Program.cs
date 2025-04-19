using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using RahmanMemberVault.Api.Extensions;
using RahmanMemberVault.Infrastructure.Data;
using Swashbuckle.AspNetCore.SwaggerGen;
using Serilog;


// Configure Serilog for logging
Directory.CreateDirectory("ExceptionLogs");

// Show internal Serilog errors in console
Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine("SERILOG ERROR: " + msg));
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("ExceptionLogs/logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
Log.Information("Test log at startup"); // Force a log

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Application & Infrastructure
builder.Services
       .AddApplicationLayer()
       .AddInfrastructureLayer(builder.Configuration);

// Add AppExceptionHandler
builder.Services.AddExceptionHandler<AppExceptionHandler>();
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


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RahmanMemberVault API V1");
    });
//}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
