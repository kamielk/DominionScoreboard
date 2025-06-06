using System.Diagnostics;

namespace Scoreboard.Cards;

[DebuggerDisplay("Name = {Name}")]
public class DominionSetFile
{
    /// <summary>
    /// Name of the set
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// The different cards in the file
    /// </summary>
    public IEnumerable<CardRepresentation> Cards { get; init; } = null!;
}