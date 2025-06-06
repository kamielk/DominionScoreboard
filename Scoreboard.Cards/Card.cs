using System.Diagnostics;
using Scoreboard.Cards.Extensions;

namespace Scoreboard.Cards;

[DebuggerDisplay("Name = {Name}")]
public record Card(string Name, string Set, IDictionary<string, int> Cost, IEnumerable<string> Types) : ICard
{
    public string ImageUrl => $"/images/{Set.FormatName()}/{Name.FormatName()}.jpg";
};
