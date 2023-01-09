using System.Diagnostics;

namespace Scoreboard.Cards;

[DebuggerDisplay("Name = {Name}")]
internal class DominionSetFile
{
    /// <summary>
    /// Name of the set
    /// </summary>
    public string Name { get; init; } = null!;

    /// <summary>
    /// The different cards in the file
    /// </summary>
    public IEnumerable<CardRepresentation> Cards { get; init; } = null!;
}