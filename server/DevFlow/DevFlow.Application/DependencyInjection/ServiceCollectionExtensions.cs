using DevFlow.Application.Interfaces;
using DevFlow.Application.Services;
using DevFlow.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DevFlow.Application.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ITaskService, TaskService>()
            .AddScoped<IDependencyEngine, DependencyEngine>();
    }
}
