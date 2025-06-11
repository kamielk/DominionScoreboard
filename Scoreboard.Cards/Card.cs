using System.Diagnostics;
using Scoreboard.Cards.Extensions;

namespace Scoreboard.Cards;

[DebuggerDisplay("Name = {Name}")]
public record Card(string Name, string Set, IDictionary<string, int> Cost, IEnumerable<string> Types, Func<IEnumerable<CardAndCount>, ICard, int>? GetVp = null, bool OnTavernMat = false) : ICard
{
    public string ImageUrl => $"/images/{Set.FormatName()}/{Name.FormatName()}.jpg";

    public bool IsOnTavernMat => OnTavernMat;

    public int GetVictoryPoints(IEnumerable<CardAndCount> countByCard)
    {
        if (GetVp is null) return 0;

        return GetVp(countByCard, this);
    }
};
