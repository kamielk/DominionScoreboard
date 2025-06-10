namespace Scoreboard.Cards;

public class PlayerDeck(IEnumerable<CardAndCount> Cards)
{
    public List<CardAndCount> CardAndCounts { get; set; } = [.. Cards];

    public int CountAll() => CardAndCounts.Aggregate(0, (total, cardAndCount) => total + cardAndCount.Count);

    public int CountByName(string name)
    {
        return CardAndCounts
            .Where(c => c.Card.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            .Aggregate(0, (total, cardAndCount) => total + cardAndCount.Count);
    }

    public int CountByType(string type)
    {
        return CardAndCounts
            .Where(c => c.Card.Types.Contains(type, StringComparer.OrdinalIgnoreCase))
            .Aggregate(0, (total, cardAndCount) => total + cardAndCount.Count);
    }

    public int CountDistinctNames()
    {
        return CardAndCounts.Select(c => c.Card.Name).Distinct(StringComparer.OrdinalIgnoreCase).Count();
    }
}
