using System.Diagnostics;

namespace Scoreboard.Cards;


/// <summary>
/// a single card in a set file
/// </summary>
[DebuggerDisplay("{Name}")]
public record CardRepresentation(string Name, IDictionary<string, int> Cost, string? VpAlg, string[] Types, string[] Abilities);