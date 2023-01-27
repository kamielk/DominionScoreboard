using System.Diagnostics;

namespace Scoreboard.Cards;

/// <summary>
/// A set of dominion cards
/// </summary>
[DebuggerDisplay("{Name}")]
public class DominionSet
{
    /// <summary>
    /// Name of the set
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The different <see cref="ICard"/>s in this set
    /// </summary>
    public IEnumerable<ICard> Cards { get; init; } = null!;
}