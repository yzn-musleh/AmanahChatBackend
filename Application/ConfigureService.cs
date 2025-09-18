using Application.Common;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ConfigureService
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ConfigureService).GetTypeInfo().Assembly));

            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UserContextBehavior<,>));

            return services;
        }
    }
}
