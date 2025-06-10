namespace Scoreboard.Cards;

/// <summary>
/// Can load the different cards from a Dominion expansion-file
/// </summary>
public interface IDominionSetManager
{
    /// <summary>
    /// Loads all the available <see cref="DominionSet"/>s
    /// </summary>
    public IEnumerable<DominionSet> GetAllSets();
    
    /// <summary>
    /// Loads the base cards
    /// </summary>
    /// <returns></returns>
    public IEnumerable<ICard> GetBaseCards();

    /// <summary>
    /// Gets a single set with given <paramref name="name"/>
    /// </summary>
    /// <param name="name">name of the set to get</param>
    /// <returns>set with given <paramref name="name"/> or null when it's not available</returns>
    public DominionSet? GetSet(string name);
}