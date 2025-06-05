using System.Diagnostics;

namespace Scoreboard.Cards;

[DebuggerDisplay("Name = {Name}")]
public record Card(string Name, string Expansion, IDictionary<string, int> Cost, IEnumerable<string> Types) : ICard;
