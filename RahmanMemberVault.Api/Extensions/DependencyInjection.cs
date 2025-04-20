using Microsoft.EntityFrameworkCore;
using RahmanMemberVault.Infrastructure.Data;
using RahmanMemberVault.Application.Mapping;
using RahmanMemberVault.Application.Interfaces;
using RahmanMemberVault.Application.Services;
using RahmanMemberVault.Core.Interfaces;
using RahmanMemberVault.Infrastructure.Repositories;
using FluentValidation.AspNetCore;
using FluentValidation;
using RahmanMemberVault.Application.Validators;
using System;

namespace RahmanMemberVault.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // AutoMapper configuration - scan for profiles in the Application.Mapping assembly
            services.AddAutoMapper(typeof(MemberMappingProfile).Assembly);

            // Application services
            services.AddScoped<IMemberService, MemberService>();

            // FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<CreateMemberDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateMemberDtoValidator>();

            return services;
        }
        public static IServiceCollection AddInfrastructureLayer(
            this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            // ensure App_Data folder exists
            var dataDir = Path.Combine(environment.ContentRootPath, "App_Data");
            Directory.CreateDirectory(dataDir);

            // read the connection string from config
            var conn = configuration.GetConnectionString("MemberVaultDb");
            services.AddDbContext<ApplicationDbContext>(opts =>
                opts.UseSqlite(conn));

            // Repository layer
            services.AddScoped<IMemberRepository, MemberRepository>();

            return services;
        }

    }
}
