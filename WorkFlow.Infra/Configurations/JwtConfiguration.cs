using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkFlow.Application.Settings;

namespace WorkFlow.Infra.Configurations
{
    public static class JwtConfiguration
    {
        public static IServiceCollection AddJwtConfiguration(this IServiceCollection service, JwtSettings settings)
        {
            // Verificação de segurança (Fail Fast)
            if (string.IsNullOrEmpty(settings?.Key))
            {
                throw new InvalidOperationException("A chave JWT não foi configurada no appsettings.json.");
            }
            var key = Encoding.UTF8.GetBytes(settings.Key);

            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.
                AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.
                AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = settings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = settings.Audience,
                        ClockSkew = TimeSpan.Zero

                    };
                });
            service.AddAuthorization();
            return service;
        }
    }
}
