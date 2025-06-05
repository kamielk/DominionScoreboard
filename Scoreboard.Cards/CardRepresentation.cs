using System.Diagnostics;

namespace Scoreboard.Cards;

/// <summary>
/// a single card in a set file
/// </summary>
[DebuggerDisplay("{Name}")]
public class CardRepresentation
{
    public string Name { get; init; } = null!;
    public string Expansion { get; init; } = null!;
    public IDictionary<string, int> Cost { get; init; } = null!;
    public IEnumerable<string> Types { get; init; } = null!;
    
    public string? VpAlgorithm { get; set; }
}
