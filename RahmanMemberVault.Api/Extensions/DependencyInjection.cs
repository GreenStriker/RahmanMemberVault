using Microsoft.EntityFrameworkCore;
using RahmanMemberVault.Infrastructure.Data;
using RahmanMemberVault.Application.Mapping;

namespace RahmanMemberVault.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // AutoMapper configuration - scan for profiles in the Application.Mapping assembly
            services.AddAutoMapper(typeof(MemberMappingProfile).Assembly);
            return services;
        }
        public static IServiceCollection AddInfrastructureLayer(
            this IServiceCollection services, IConfiguration configuration)
        {
            //Ef Core Sqlite database context
            services.AddDbContext<ApplicationDbContext>((provider, options) =>
            {
                // get the IWebHostEnvironment from DI
                var env = provider.GetRequiredService<IWebHostEnvironment>();
                var dbPath = Path.Combine(env.ContentRootPath, "App_Data", "members.db");
                options.UseSqlite($"Data Source={dbPath}");
            });


            return services;
        }

    }
}
