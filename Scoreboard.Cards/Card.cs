using System.Diagnostics;
using Scoreboard.Cards.Algorithms;

namespace Scoreboard.Cards;

[DebuggerDisplay("Name = {Name}")]
public class Card : ICard
{
    public string? Name { get; init; }
    public string? Expansion { get; init; }
    public IDictionary<string, int>? Cost { get; init; }
    public IEnumerable<string>? Types { get; init; }

    /// <summary>
    /// The <see cref="IVpAlgorithm"/>s used to calculate how many victory points this card gives 
    /// </summary>
    public IEnumerable<IVpAlgorithm> Algorithms { get; init; } = new List<IVpAlgorithm>();
    
    public virtual int GetVictoryPoints(ICollection<ICard> playerCards)
    {
        if (Algorithms is null) return 0;
        return Algorithms.Sum(algorithm => algorithm.GetVictoryPoints(playerCards));
    }
}
