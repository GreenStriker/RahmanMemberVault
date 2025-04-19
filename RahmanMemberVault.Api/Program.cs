using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using RahmanMemberVault.Api.Extensions;
using RahmanMemberVault.Infrastructure.Data;
using Swashbuckle.AspNetCore.SwaggerGen;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Application & Infrastructure
builder.Services
       .AddApplicationLayer()
       .AddInfrastructureLayer(builder.Configuration);
//

builder.Services.AddExceptionHandler<AppExceptionHandler>();

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

var app = builder.Build();

// Deploy the database in app statrup from Migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RahmanMemberVault API V1");
    });
}

app.UseExceptionHandler(_ => { });      

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
