using Microsoft.Extensions.DependencyInjection;
using WorkFlow.Application.Commands.ApproveRequest;
using WorkFlow.Application.Commands.Auth;
using WorkFlow.Application.Commands.CreateRequest;
using WorkFlow.Application.Commands.RejectRequest;

namespace WorkFlow.Application.DepencicyInjection
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection services)
        {
            services.AddScoped<LoginCommandHandler>();
            services.AddScoped<CreateRequestCommandHandler>();
            services.AddScoped<ApproveRequestCommandHandler>();
            services.AddScoped<RejectionRequestCommandHandler>();
            return services;
        }
    }
}
