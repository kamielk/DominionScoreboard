using Dominion.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Scoreboard.Cards;
using Scoreboard.Data.Config;

namespace Dominion.Services;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the necessary configuration and services
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config">the configuration root</param>
    public static void RegisterDominionScoreboardService(this IServiceCollection services, IConfiguration config)
    {
        // bind DominionScoreboardDatabaseSettings config
        var section = config.GetRequiredSection(DominionScoreboardDatabaseConfig.DominionScoreboardDatabase);
        services.Configure<DominionScoreboardDatabaseConfig>(section);

        // register MongoDb
        services.AddScoped<IMongoCollection<Game>>(sp =>
        {
            var dbSettings = sp.GetRequiredService<IOptions<DominionScoreboardDatabaseConfig>>().Value;
            var mongoClient = new MongoClient(dbSettings.ConnectionString);
            var mongoDb = mongoClient.GetDatabase(dbSettings.DatabaseName);
            return mongoDb.GetCollection<Game>(dbSettings.GamesCollectionName);
        });

        services.AddScoped<IGamesService, GamesService>();

        services.RegisterDominionExpansionManager();
    }
}