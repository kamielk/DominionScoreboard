namespace Scoreboard.Cards.Algorithms;

public abstract class VpAlgorithm : IVpAlgorithm
{
    public abstract string Name { get; }
    public abstract int GetVictoryPoints(ICollection<ICard> playerCards);
}

public class CountAlgorithm : VpAlgorithm
{
    private readonly double _modifier;
    private readonly Func<ICard, bool> _filter;

    /// <summary>
    /// Creates an instance of <see cref="CountAlgorithm"/>
    /// </summary>
    /// <param name="modifier">the number by which the count is multiplied, this is always rounded down</param>
    /// <param name="filter">the requirement of a card to get counted</param>
    public CountAlgorithm(double modifier, Func<ICard, bool> filter)
    {
        _modifier = modifier;
        _filter = filter ?? throw new ArgumentNullException(nameof(filter));
    }

    public override string Name => VpAlgorithmNames.Count;

    public override int GetVictoryPoints(ICollection<ICard> playerCards)
    {
        return Convert.ToInt32((Math.Floor(playerCards.Count(card => _filter(card)) * _modifier)));
    }
}