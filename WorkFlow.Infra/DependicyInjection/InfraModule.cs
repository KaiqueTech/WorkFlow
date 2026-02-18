using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkFlow.Application.Abstractions;
using WorkFlow.Application.Interfaces;
using WorkFlow.Application.Queries;
using WorkFlow.Application.Settings;
using WorkFlow.Domain.Interfaces;
using WorkFlow.Domain.Models;
using WorkFlow.Infra.Configurations;
using WorkFlow.Infra.Persistence.Read.Connection;
using WorkFlow.Infra.Persistence.Read.Repositories;
using WorkFlow.Infra.Persistence.Write.Context;
using WorkFlow.Infra.Persistence.Write.Repositories;
using WorkFlow.Infra.Services;

namespace WorkFlow.Infra.DependicyInjection
{
    public static class InfraModule
    {
        public static IServiceCollection AddInfraModule(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSection);

            var jwtSettings = jwtSection.Get<JwtSettings>();
            services.AddSingleton(jwtSettings!);

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUserModel, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddScoped<IJwtService, JwtTokenService>();
            services.AddScoped<IRequestQueriesRepository, RequestQueriesRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<IDbConnectionFactory, SqlConnectionFactory>();

            services.AddJwtConfiguration(jwtSettings!);

            return services;
        }
    }
}