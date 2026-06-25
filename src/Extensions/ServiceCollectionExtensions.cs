using Microsoft.Extensions.DependencyInjection;
using TeamsNotifier.Services;

namespace TeamsNotifier.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTeamsNotifier(this IServiceCollection services)
    {
        services.AddHttpClient<ITeamsNotifierService, TeamsNotifierService>();
        return services;
    }
}
