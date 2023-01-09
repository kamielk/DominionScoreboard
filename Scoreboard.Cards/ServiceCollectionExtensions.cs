using Microsoft.Extensions.DependencyInjection;

namespace Scoreboard.Cards;

public static class ServiceCollectionExtensions
{
    public static void RegisterDominionExpansionManager(this IServiceCollection services)
    {
        // register all IDominionExpansionManagers
        services.AddSingleton<IDominionSetManager, DominionSetManager>();
    }
}