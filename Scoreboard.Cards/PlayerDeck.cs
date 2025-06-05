namespace Scoreboard.Cards;

public class PlayerDeck
{
    public List<Card> Cards { get; set; } = [];

    public int CountAll() => Cards.Count;

    public int CountByName(string name)
    {
        return Cards.Count(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }

    public int CountByType(string type)
    {
        return Cards.Count(c => c.Types.Contains(type, StringComparer.OrdinalIgnoreCase));
    }

    public int CountDistinctNames()
    {
        return Cards.Select(c => c.Name).Distinct(StringComparer.OrdinalIgnoreCase).Count();
    }
}
