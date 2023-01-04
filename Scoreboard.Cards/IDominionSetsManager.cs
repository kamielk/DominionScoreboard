namespace Scoreboard.Cards;

/// <summary>
/// Can load the different cards from a Dominion expansion-file
/// </summary>
public interface IDominionSetsManager
{
    /// <summary>
    /// Loads the different cards from an expansion
    /// </summary>
    /// <param name="dominionSetYml">path to a Dominion expansion-file</param>
    public IEnumerable<ICard> GetExpansion(string dominionSetYml);
}