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

    /// <summary>
    /// How many points this card innately gives
    /// </summary>
    public int VictoryPoints { get; set; }
    
    public VpAlgorithmRepresentation VpAlgorithm { get; set; }

    public class VpAlgorithmRepresentation
    {
        public string Name { get; set; }
        public double Modifier { get; set; } = 0;
        public string Filter { get; set; }
    }
}