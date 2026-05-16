using DevFlow.Domain.Interfaces;
using DevFlow.Infrastructure.GitHub;
using Microsoft.Extensions.DependencyInjection;
using Octokit;

namespace DevFlow.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGitHubAdapter(
        this IServiceCollection services,
        string productName,
        string? token = null)
    {
        services.AddSingleton(serviceProvider =>
        {
            var client = new GitHubClient(new ProductHeaderValue(productName));
            if (!string.IsNullOrWhiteSpace(token))
            {
                client.Credentials = new Credentials(token);
            }

            return client;
        });

        return services.AddScoped<IGitHubAdapter, GitHubAdapter>();
    }
}

