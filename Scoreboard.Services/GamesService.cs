using Dominion.Data;
using MongoDB.Driver;

namespace Dominion.Services;

public class GamesService : IGamesService
{
    private readonly IMongoCollection<Game> _gamesCollection;
    
    public GamesService(IMongoCollection<Game> gamesCollection)
    {
        _gamesCollection = gamesCollection ?? throw new ArgumentNullException(nameof(gamesCollection));
    }
    
    public async Task<List<Game>> GetAsync()
    {
        return await _gamesCollection.Find(_ => true).ToListAsync();
    }

    public async Task<Game?> GetAsync(string id)
    {
        return await _gamesCollection.Find(game => game.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Game newGame)
    {
        await _gamesCollection.InsertOneAsync(newGame);
    }

    public async Task UpdateAsync(string id, Game updatedGame)
    {
        await _gamesCollection.ReplaceOneAsync(game => game.Id == id, updatedGame);
    }

    public async Task RemoveAsync(string id)
    {
        await _gamesCollection.DeleteOneAsync(game => game.Id == id);
    }
}