using Dominion.Data;

namespace Dominion.Services;

/// <summary>
/// Manages dominion <see cref="Game"/>s
/// </summary>
public interface IGamesService
{
    /// <summary>
    /// Gets all <see cref="Game"/>s
    /// </summary>
    /// <returns>a collection of all available games</returns>
    public Task<List<Game>> GetAsync();

    /// <summary>
    /// Gets a <see cref="Game"/>
    /// </summary>
    /// <param name="id">a <see cref="Game"/>'s unique identifier</param>
    /// <returns>a game or null when a <see cref="Game"/> with <paramref name="id"/> is not found</returns>
    public Task<Game?> GetAsync(string id);

    /// <summary>
    /// Creates a new <see cref="Game"/>
    /// </summary>
    /// <param name="game">a dominion game</param>
    /// <returns>the id of the created <see cref="Game"/></returns>
    public Task CreateAsync(Game newGame);

    /// <summary>
    /// Updates an existing <see cref="Game"/>
    /// </summary>
    /// <param name="id">a <see cref="Game"/>'s unique identifier</param>
    /// <param name="game">a dominion game</param>
    public Task UpdateAsync(string id, Game updatedGame);

    /// <summary>
    /// Deletes a <see cref="Game"/>
    /// </summary>
    /// <param name="id">unique identifier of the <see cref="Game"/> to remove</param>
    public Task RemoveAsync(string id);
}