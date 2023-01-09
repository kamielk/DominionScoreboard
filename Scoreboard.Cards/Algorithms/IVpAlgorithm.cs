namespace Scoreboard.Cards.Algorithms;

public interface IVpAlgorithm
{
    /// <summary>
    /// This algorithm's name
    /// </summary>
    string Name { get; }
    
    /// <summary>
    /// Gets the amount of victory points for this algorithm
    /// </summary>
    /// <param name="playerCards">The player's cards</param>
    /// <returns></returns>
    int GetVictoryPoints(ICollection<ICard> playerCards);
}